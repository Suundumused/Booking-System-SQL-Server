CREATE OR ALTER FUNCTION fn_ListBookingsByHotel
(
	@HotelID INT = 1
)
RETURNS TABLE
AS
RETURN
(
	SELECT
		b.ID AS 'Booking ID',
		u.Name AS 'Owner',
		u.Username AS 'Username',
		u.Email,
		b.CheckIn AS 'Check-In',
		b.CheckOut AS 'Check-Out',
		b.Price AS 'Total Price'
	FROM Bookings b
		INNER JOIN Hotels h
	ON h.ID = b.HotelID
		INNER JOIN Users u
	ON u.ID = b.UserID
	WHERE
		h.ID = @HotelID
);
GO