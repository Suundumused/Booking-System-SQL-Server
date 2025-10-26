CREATE OR ALTER TRIGGER trg_Check_Booking_Overlaps
ON Bookings
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ErrorMessage NVARCHAR(150);

	DECLARE @CheckIn DATETIME;
	DECLARE @CheckOut DATETIME;

	DECLARE @HotelID INT;

	SELECT @HotelID = HotelID, @CheckIn = CheckIn, @CheckOut = CheckOut FROM inserted;

	EXEC usp_DateRangeChecker @CheckIn, @CheckOut;

	DECLARE @MaxRooms INT;
	SELECT @MaxRooms = Rooms FROM Hotels WHERE ID = @HotelID;

	EXEC usp_CheckBookingsBlocksOverlaps @HotelID, @MaxRooms, -1, @CheckIn, @CheckOut

	INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID, Price)
	OUTPUT 
		inserted.ID, 
		inserted.Price
	SELECT 
		CheckIn, 
		CheckOut, 
		UserID, 
		HotelID, 
		(
			SELECT dbo.fn_PriceSetter
			(
				@HotelID, 
				@CheckIn, 
				@CheckOut, 
				(SELECT Price FROM inserted)
			)
		)
	FROM inserted;
END;
GO