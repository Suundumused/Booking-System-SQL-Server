CREATE OR ALTER TRIGGER trg_Check_Booking_Overlaps_On_Update
ON Bookings
INSTEAD OF UPDATE
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SelfID INT;

	DECLARE @ErrorMessage NVARCHAR(150);

	DECLARE @CheckIn DATETIME;
	DECLARE @CheckOut DATETIME;

	DECLARE @HotelID INT;
	DECLARE @UserID INT;

	SELECT @SelfID = ID, @UserID = UserID, @HotelID = HotelID, @CheckIn = CheckIn, @CheckOut = CheckOut FROM inserted;

	EXEC usp_DateRangeChecker @CheckIn, @CheckOut;

	DECLARE @MaxRooms INT;
	SELECT @MaxRooms = Rooms FROM Hotels WHERE ID = @HotelID;

	EXEC usp_CheckBookingsBlocksOverlaps @HotelID, @MaxRooms, @SelfID, @CheckIn, @CheckOut

	UPDATE Bookings
	SET 
		CheckIn = @CheckIn,
		CheckOut = @CheckOut,
		UserID = @UserID,
		HotelID = @HotelID,
		Price = (
			SELECT dbo.fn_PriceSetter
			(
				@HotelID, 
				@CheckIn, 
				@CheckOut, 
				(
					SELECT 
					CASE 
						WHEN UPDATE(Price) THEN (SELECT Price FROM inserted)
						ELSE NULL
					END
				)
			)
		)
	OUTPUT 
		inserted.Price
	WHERE Bookings.ID = @SelfID;
END;
GO