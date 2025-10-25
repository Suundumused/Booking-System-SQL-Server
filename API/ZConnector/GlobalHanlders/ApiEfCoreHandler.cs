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
            TypeMismatch
        }

        private static void EfErrorStatement(DbUpdateException ex, string objectName) 
        {
            EfErrorType type = GetErrorType(ex);

            Exception? errMessage = ex.InnerException;
            if (errMessage != null) { Debug.Print(errMessage.Message); }

            (string, int) result = type switch
            {
                EfErrorType.DuplicateKey => (objectName + " already exists.", 401),
                EfErrorType.ForeignKey => (objectName + " not found for this occurrence.", 404),
                EfErrorType.NullConstraint => ($"A required field for {objectName} is missing.", 400),
                EfErrorType.TypeMismatch => ($"Invalid data format. Target is {objectName}.", 400),
                EfErrorType.AccessDenied => ($"Permission denied when accessing {objectName} data.", 400),
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
                        return EfErrorType.DuplicateKey; // Duplicate key

                    case 547: 
                        return EfErrorType.ForeignKey; // Foreign key constraint violation

                    case 515: 
                        return EfErrorType.NullConstraint; // Cannot insert null into not-null column

                    case 5470: 
                        return EfErrorType.CheckConstraint; // Check constraint

                    case 229: 
                        return EfErrorType.AccessDenied; // Permission denied

                    case 245:
                    case 8114: 
                        return EfErrorType.TypeMismatch; // Error converting data type

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
