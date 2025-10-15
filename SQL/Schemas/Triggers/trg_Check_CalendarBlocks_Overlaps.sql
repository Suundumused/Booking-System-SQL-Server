CREATE OR ALTER TRIGGER trg_Check_CalendarBlocks_Overlaps
ON CalendarBlocks
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ErrorMessage NVARCHAR(150);

	DECLARE @DateIn DATETIME;
	DECLARE @DateOut DATETIME;

	SELECT @DateIn = DateIn, @DateOut = DateOut FROM inserted;

	EXEC usp_DateTimeRangeChecker @DateIn, @DateOut;

	DECLARE @HotelID INT = (SELECT HotelID FROM inserted);

	IF EXISTS
	(
		SELECT 1
			FROM CalendarBlocks
		WHERE
			HotelID = @HotelID
		AND
			DateIn < @DateOut
		AND
			DateOut > @DateIn
	)
	BEGIN
		SET @ErrorMessage = 'The date block overlaps an existing one.';
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END
		ELSE
	BEGIN
		IF EXISTS
		(
			SELECT 1
				FROM Bookings 
			WHERE
				HotelID = @HotelID
			AND
				CheckIn < @DateOut
			AND
				CheckOut > @DateIn
		)
		BEGIN
			SET @ErrorMessage = 'The date block overlaps an existing booking. From ';
			THROW 51000, @ErrorMessage, 1;
			RETURN;
		END;
	END;

	INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) 
	VALUES (@HotelID, @DateIn, @DateOut);
END;
GO