CREATE OR ALTER PROCEDURE usp_DateRangeChecker
(
	@DateIn DATE,
	@DateOut DATE
)
AS
BEGIN
	IF @DateIn >= @DateOut
	BEGIN
		DECLARE @ErrorMessage NVARCHAR = 'The exit Time and Date must be later than the entry date.';
		THROW 51000, @ErrorMessage, 1;
		RETURN;
	END;
END;
GO