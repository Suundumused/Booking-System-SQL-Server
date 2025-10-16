CREATE OR ALTER FUNCTION fn_Login
(
	@PasswordHash VARCHAR(256),
	@Salt VARCHAR(256)
)
RETURNS INT
AS
BEGIN
	RETURN COALESCE (
		(
			SELECT 
				ID
			FROM 
				Users
			WHERE PasswordHash = @PasswordHash AND Salt = @Salt
		), 
		-1
	);
END;
GO