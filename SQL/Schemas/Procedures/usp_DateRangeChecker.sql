CREATE OR ALTER PROCEDURE usp_DateRangeChecker
(
	@DateIn DATE,
	@DateOut DATE
)
AS
BEGIN
	IF @DateIn >= @DateOut
	BEGIN
		DECLARE @ErrorMessage NVARCHAR = 'The exit Date must be later than the entry date.';
		THROW 65470, @ErrorMessage, 1;
		RETURN;
	END;
END;
GO