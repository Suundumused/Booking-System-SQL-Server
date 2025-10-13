CREATE OR ALTER VIEW VW_Bookings_Users_Hotel_CalendarBlocks
AS
SELECT
	h.Name AS 'Hotel Name',
	u.Email AS 'User Email',
	b.CheckIn AS 'Booking Check-In',
	b.CheckOut	AS 'Booking Check-Out',
	c.DateIn AS 'Locked Start Date',
	c.DateOut AS 'Locked End Date'
FROM Bookings b
	INNER JOIN Hotels h ON h.ID = b.HotelID
	INNER JOIN Users u ON u.ID = b.UserID
	LEFT JOIN CalendarBlocks c ON c.HotelID = b.HotelID;
GO