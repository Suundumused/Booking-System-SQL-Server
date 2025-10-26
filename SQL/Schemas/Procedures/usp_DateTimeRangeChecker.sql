CREATE OR ALTER PROCEDURE usp_DateTimeRangeChecker
(
	@DateIn DATETIME,
	@DateOut DATETIME
)
AS
BEGIN
	IF @DateIn > @DateOut
	BEGIN
		DECLARE @ErrorMessage NVARCHAR = 'The exit Time and Date must be later than the entry date.';
		THROW 65470, @ErrorMessage, 1;
		RETURN;
	END;
END;
GO