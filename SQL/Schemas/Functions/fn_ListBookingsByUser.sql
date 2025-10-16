CREATE OR ALTER FUNCTION fn_ListBookingsByUser
(
	@Email NVARCHAR(100) = NULL,
	@Username NVARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(	
	SELECT
		b.ID AS 'Booking ID',
		h.Name AS 'Hotel Name',
		b.CheckIn AS 'Check-In',
		b.CheckOut AS 'Check-Out',
		b.Price AS 'Total Price'
	FROM Bookings b 
		INNER JOIN Hotels h 
	ON h.ID = b.HotelID
		INNER JOIN Users u
	ON u.ID = b.UserID
	WHERE
		(@Email IS NOT NULL AND u.Email = @Email)
		OR
		(@Username IS NOT NULL AND u.Username = @Username)
);
GO