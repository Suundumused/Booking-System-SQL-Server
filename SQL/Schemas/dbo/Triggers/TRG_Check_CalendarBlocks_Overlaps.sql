CREATE TRIGGER TRG_Check_CalendarBlocks_Overlaps
ON CalendarBlocks
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ErrorMessage VARCHAR(150);

	DECLARE @HotelID INT;

	DECLARE @DateIn DATETIME;
	DECLARE @DateOut DATETIME;
	DECLARE @OverlappedDateIn DATETIME;
	DECLARE @OverlappedDateOut DATETIME;

	SELECT @HotelID = HotelID, @DateIn = DateIn, @DateOut = DateOut FROM inserted;

	SELECT TOP 1
		@OverlappedDateIn = DateIn,
		@OverlappedDateOut = DateOut
	FROM CalendarBlocks
	WHERE
		HotelID = @HotelID
	AND
	(
		(@DateIn BETWEEN DateIn AND DateOut)
		OR
		(@DateOut BETWEEN DateIn AND DateOut)
		OR
		(DateIn BETWEEN @DateIn AND @DateOut)
	);

	IF @OverlappedDateIn IS NOT NULL
	BEGIN
		SET @ErrorMessage = CONCAT(
			'The date block overlaps an existing one. From ', 
			@OverlappedDateIn, 
			' to ', 
			@OverlappedDateOut
		);
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END;
		ELSE
	BEGIN
		SELECT TOP 1
			@OverlappedDateIn = COALESCE(CheckIn, NULL),
			@OverlappedDateOut = CheckOut
		FROM Bookings 
		WHERE
			HotelID = @HotelID
		AND
		(
			(@DateIn BETWEEN CheckIn AND CheckOut) 
			OR 
			(@DateOut BETWEEN CheckIn AND CheckOut)
			OR
			(CheckIn BETWEEN @DateIn AND @DateOut)
		);

		IF @OverlappedDateIn IS NOT NULL
		BEGIN
			SET @ErrorMessage = CONCAT(
				'The date block overlaps an existing booking. From ', 
				@OverlappedDateIn, 
				' to ', 
				@OverlappedDateOut
			);
			THROW 51000, @ErrorMessage, 1;
			RETURN;
		END;
	END;

	INSERT INTO CalendarBlocks (DateIn, DateOut, HotelID) VALUES (@DateIn, @DateOut, @HotelID);
END;
GO