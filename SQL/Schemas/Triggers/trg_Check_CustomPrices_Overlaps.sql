CREATE OR ALTER TRIGGER trg_Check_CustomPrices_Overlaps
ON CustomPrices
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
		FROM CustomPrices
		WHERE
			HotelID = @HotelID
		AND
			DateIn < @DateOut
		AND
			DateOut > @DateIn
	)
	BEGIN
		SET @ErrorMessage = 'The date price overlaps an existing one.';
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END;

	INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) 
	VALUES (@HotelID, @DateIn, @DateOut, (SELECT Price FROM inserted));
END;
GO