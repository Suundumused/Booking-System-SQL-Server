CREATE TRIGGER TRG_Check_Booking_Overlaps
ON Bookings
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ErrorMessage VARCHAR(150);

	DECLARE @CheckIn DATETIME;
	DECLARE @CheckOut DATETIME;
	DECLARE @BlockedDateIn DATETIME;
	DECLARE @BlockedDateOut DATETIME;

	DECLARE @HotelID INT;
	DECLARE @OverlapingCount INT;
	DECLARE @MaxRooms INT;

	SELECT @HotelID = HotelID, @CheckIn = CheckIn, @CheckOut = CheckOut FROM inserted;
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
	END;
		ELSE
	BEGIN
		SELECT TOP 1
			@BlockedDateIn = DateIn, 
			@BlockedDateOut = DateOut
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
		);

		IF @BlockedDateIn IS NOT NULL
		BEGIN
			SET @ErrorMessage = CONCAT(
				'The dates are blocked at the hotel during this period. From ', 
				@BlockedDateIn, 
				' to ', 
				@BlockedDateOut
			);
			THROW 51000, @ErrorMessage, 1;
			RETURN;
		END;
	END;

	INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) 
	SELECT CheckIn, CheckOut, UserID, HotelID 
	FROM inserted;
END;
GO