CREATE OR ALTER FUNCTION fn_PriceSetter
(
	@HotelID INT,

	@CheckIn DATETIME,
	@CheckOut DATETIME,

	@CustomPrice DECIMAL(10, 2) = NULL
)
RETURNS DECIMAL(10, 2)
BEGIN
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
		)
	FROM CustomPrices
	WHERE
		HotelID = @HotelID
	AND
		DateIn < @CheckOut
	AND
		DateOut > @CheckIn;

	DECLARE @TotalBookingDays INT = DATEDIFF(DAY, @CheckIn, @CheckOut);
	DECLARE @TotalOverlapingDays INT = (SELECT SUM(OverlapingCount) 
		FROM @OverlapingCustomPrices);

	DECLARE @FinalPrice DECIMAL(10, 2);

	IF @CustomPrice IS NOT NULL
	BEGIN
		SET @FinalPrice = @CustomPrice * @TotalBookingDays;
	END
		ELSE
	BEGIN
		IF EXISTS 
		(
			SELECT 1 FROM @OverlapingCustomPrices
		)
		BEGIN
			IF @TotalBookingDays = @TotalOverlapingDays
			BEGIN
				SET @FinalPrice = (SELECT SUM(Price * OverlapingCount) 
					FROM @OverlapingCustomPrices);
			END
				ELSE
			BEGIN
				SET @FinalPrice = 
				(SELECT SUM(Price * OverlapingCount) 
					FROM @OverlapingCustomPrices)
				+ 
				(@TotalBookingDays - @TotalOverlapingDays)
				* 
				(SELECT Price FROM Hotels WHERE ID = @HotelID);
			END;
		END
			ELSE
		BEGIN
			SET @FinalPrice = 
			@TotalBookingDays 
			* 
			(SELECT Price FROM Hotels WHERE ID = @HotelID);
		END;
	END;

	RETURN @FinalPrice;
END;
GO