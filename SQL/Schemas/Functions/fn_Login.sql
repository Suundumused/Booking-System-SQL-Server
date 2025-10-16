CREATE OR ALTER FUNCTION fn_Login
(
	@PasswordHash VARCHAR(256),
	@Salt VARCHAR(256)
)
RETURNS INT
AS
BEGIN
	DECLARE @ID INT = 
	(
		SELECT 
			ID
		FROM 
			Users
		WHERE PasswordHash = @PasswordHash AND Salt = @Salt
	);

	IF @ID IS NOT NULL
	BEGIN
		RETURN @ID;
	END;

	RETURN -1;
END;
GO