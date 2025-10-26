CREATE OR ALTER PROCEDURE usp_CheckBookingsBlocksOverlaps
(
	@HotelID INT,
	@MaxRooms INT,
	@IgnoredId INT,

	@CheckIn DATETIME,
	@CheckOut DATETIME
)
AS
BEGIN
	DECLARE @OverlapingCount INT;
	DECLARE @ErrorMessage NVARCHAR(150);

	SELECT @OverlapingCount = COUNT(ID) 
		FROM Bookings 
	WHERE
		HotelID = @HotelID
	AND
		CheckIn < @CheckOut
	AND
		CheckOut > @CheckIn
	AND 
		ID != @IgnoredId;
			 
	IF @OverlapingCount >= @MaxRooms
	BEGIN 
		SET @ErrorMessage = CONCAT(
			'Max rooms exhausted for the selected hotel. Overlaps: ', 
			@OverlapingCount
		);
		THROW 92601, @ErrorMessage, 1;
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
				DateIn < @CheckOut
			AND
				DateOut > @CheckIn
		)
		BEGIN
			SET @ErrorMessage = 'The reservation dates overlap with blocking dates.';
			THROW 102601, @ErrorMessage, 1;
			RETURN;
		END;
	END;
END;
GO