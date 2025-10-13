CREATE OR ALTER TRIGGER TRG_Check_Booking_Overlaps
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

	IF CAST(@CheckIn AS DATE) >= CAST(@CheckOut AS DATE)
	BEGIN
		SET @ErrorMessage = 'The exit date must be later than the entry date.';
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END;

	DECLARE @OverlapingCount INT;
	DECLARE @MaxRooms INT;

	SELECT @MaxRooms = Rooms FROM Hotels WHERE ID = @HotelID;

	SELECT @OverlapingCount = COUNT(*) 
	FROM Bookings 
	WHERE
		HotelID = @HotelID
	AND
	(
		(@CheckIn BETWEEN CheckIn AND CheckOut) 
		OR 
		(@CheckOut BETWEEN CheckIn AND CheckOut)
		OR
		(CheckIn BETWEEN @CheckIn AND @CheckOut)
	);
			 
	IF @OverlapingCount >= @MaxRooms
	BEGIN 
		SET @ErrorMessage = CONCAT(
			'Max rooms exhausted for the selected hotel. Overlaps: ', 
			@OverlapingCount
		);
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END
		ELSE
	BEGIN
		IF EXISTS
		(
			SELECT 1
			FROM CalendarBlocks 
			WHERE
				HotelID = @HotelID
			AND
			(
				(@CheckIn BETWEEN DateIn AND DateOut) 
				OR 
				(@CheckOut BETWEEN DateIn AND DateOut)
				OR
				(DateIn BETWEEN @CheckIn AND @CheckOut)
			)
		)
		BEGIN
			SET @ErrorMessage = 'The reservation dates overlap with blocking dates.';
			THROW 51000, @ErrorMessage, 1;
			RETURN;
		END;
	END;

	DECLARE @OverlapingCustomPrices TABLE 
	(
		Price DECIMAL(10, 2) NOT NULL,
		OverlapingCount INT NOT NULL
	);

	INSERT INTO @OverlapingCustomPrices
	SELECT
		Price,
		DATEDIFF(
			DAY,
			CASE WHEN DateIn < @CheckIn THEN @CheckIn ELSE DateIn END,
			CASE WHEN DateOut > @CheckOut THEN @CheckOut ELSE DateOut END
		) + 1
	FROM CustomPrices
	WHERE
		HotelID = @HotelID
	AND
	(
		(@CheckIn BETWEEN DateIn AND DateOut)
		OR 
		(@CheckOut BETWEEN DateIn AND DateOut)
		OR 
		(DateIn BETWEEN @CheckIn AND @CheckOut)
	);

	DECLARE @TotalBookingDays INT = DATEDIFF(DAY, @CheckIn, @CheckOut) + 1;
	DECLARE @TotalOverlapingDays INT = (SELECT SUM(OverlapingCount) FROM @OverlapingCustomPrices);

	DECLARE @FinalPrice DECIMAL(10, 2) = (SELECT Price FROM inserted);

	IF @FinalPrice IS NULL
	BEGIN
		IF @TotalBookingDays = @TotalOverlapingDays
		BEGIN
			SET @FinalPrice = (SELECT SUM(Price) FROM @OverlapingCustomPrices);
		END
			ELSE
		BEGIN
			SET @FinalPrice = 
			(SELECT SUM(Price * OverlapingCount) FROM @OverlapingCustomPrices)
			+ 
			(@TotalBookingDays - @TotalOverlapingDays)
			* 
			(SELECT Price FROM Hotels WHERE ID = @HotelID);
		END;
	END;

	INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID, Price)
		SELECT CheckIn, CheckOut, UserID, HotelID, @FinalPrice
	FROM inserted;
END;
GO