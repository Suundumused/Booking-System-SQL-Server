using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;


namespace ZConnector.GlobalHanlders
{
    public static class ApiEfCoreHandler
    {
        private enum EfErrorType
        {
            Unknown,
            DuplicateKey,
            ForeignKey,
            NullConstraint,
            CheckConstraint,
            AccessDenied,
            TypeMismatch,
            DateBlockOverlaps,
            DateBlockOverlapsBooking,
            PriceOverlaps,
            RoomsExhausted,
            BookingOverlapsBlock,
            DateRange,
            DateTimeRange
        }

        private static void EfErrorStatement(DbUpdateException ex, string objectName) 
        {
            EfErrorType type = GetErrorType(ex);

            (string, int) result;
            string? errMessage = ex.InnerException?.Message;

            Debug.Print("As Error: " + ex.Message);

            result = type switch
            {
                EfErrorType.DuplicateKey => (objectName + " already exists.", 401),
                EfErrorType.ForeignKey => (objectName + " not found for this occurrence.", 404),
                EfErrorType.NullConstraint => ($"A required field for {objectName} is missing.", 400),
                EfErrorType.TypeMismatch => ($"Invalid data format. Target is {objectName}.", 400),
                EfErrorType.AccessDenied => ($"Permission denied when accessing {objectName} data.", 400),
                EfErrorType.DateBlockOverlaps => ("The date block overlaps an existing one.", 409),
                EfErrorType.DateBlockOverlapsBooking => ("The date block overlaps an existing booking.", 409),
                EfErrorType.PriceOverlaps => ("The date price overlaps an existing one.", 409),
                EfErrorType.RoomsExhausted => ("Max rooms exhausted for the selected hotel.", 529),
                EfErrorType.BookingOverlapsBlock => ("The reservation dates overlap with blocking dates.", 409),
                EfErrorType.DateRange => ("The exit Date must be later than the entry date.", 400),
                EfErrorType.DateTimeRange => ("The exit Time and Date must be later than the entry date.", 400),
                _ => ($"An unexpected database error occurred. Target is {objectName}", 500)
            };

            throw new EfSafeException(message: result.Item1, statusCode: result.Item2);
        }

        private static EfErrorType GetErrorType(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                switch (sqlEx.Number)
                {
                    case 2627:
                    case 2601: 
                        return EfErrorType.DuplicateKey; 

                    case 547: 
                        return EfErrorType.ForeignKey;

                    case 515: 
                        return EfErrorType.NullConstraint; // Cannot insert null into not-null column

                    case 5470: 
                        return EfErrorType.CheckConstraint;

                    case 229: 
                        return EfErrorType.AccessDenied;

                    case 245:
                    case 8114: 
                        return EfErrorType.TypeMismatch;
                    
                    case 62601:
                        return EfErrorType.DateBlockOverlaps;

                    case 72601:
                        return EfErrorType.DateBlockOverlapsBooking;

                    case 82601:
                        return EfErrorType.PriceOverlaps;

                    case 92601:
                        return EfErrorType.RoomsExhausted;

                    case 102601:
                        return EfErrorType.BookingOverlapsBlock;

                    case 65470:
                        return EfErrorType.DateRange;

                    case 65471:
                        return EfErrorType.DateTimeRange;

                    default:
                        return EfErrorType.Unknown;
                }
            }

            return EfErrorType.Unknown;
        }

        public static async Task<T?> ExecuteWithHandlingAsync<T>(
        Func<Task<T>> func,
        string objectName,
        Action<Exception>? onException = null,

        T? defaultValue = default)
        {
            try
            {
                return await func();
            }
            catch (DbUpdateException ex)
            {
                EfErrorStatement(ex, objectName);
                return default(T);
            }
        }

        public static async Task ExecuteWithHandlingAsync( //no return
            Func<Task> func,
            string objectName,
            Action<Exception>? onException = null)
        {
            try
            {
                await func();
            }
            catch (DbUpdateException ex)
            {
                EfErrorStatement(ex, objectName);
            }
        }
    }

    public class EfSafeException : Exception
    {
        public int statusCode { get; set; }

        public EfSafeException() { }
        public EfSafeException(string message) : base(message) { }
        public EfSafeException(string message, int statusCode) : base(message) 
        {
            this.statusCode = statusCode;
        }
    }
}
