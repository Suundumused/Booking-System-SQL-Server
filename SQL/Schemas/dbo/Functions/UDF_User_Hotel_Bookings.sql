CREATE OR ALTER FUNCTION UDF_User_Hotel_Bookings 
(
	@UserEmail VARCHAR(100), 
	@HotelID INT
)
RETURNS TABLE
AS
RETURN
(
	SELECT
		u.Name AS 'User Real Name',
		u.Phone1 as 'Mobile Number',
		u.Phone2 as 'Phone Number',
		h.Name as 'Hotel Name',
		b.CheckIn as 'Booking CheckIn',
		b.CheckOut as 'Booking CheckOut'
	FROM Bookings b
		INNER JOIN Hotels h ON h.ID = b.HotelID 
		INNER JOIN Users u ON u.ID = b.UserID
	WHERE 
		u.Email = @UserEmail
		AND
		h.ID = @HotelID
);
GO