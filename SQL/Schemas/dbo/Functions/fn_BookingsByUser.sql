CREATE OR ALTER FUNCTION fn_ListBookingsByUser
(
	@Email VARCHAR(100) = NULL,
	@Username VARCHAR(100) = NULL
)
RETURNS TABLE
AS
RETURN
(	
	SELECT
		b.ID as 'Booking ID',
		h.Name as 'Hotel Name',
		b.CheckIn as 'Check-In',
		b.CheckOut as 'Check-Out',
		b.Price as 'Total Price'
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