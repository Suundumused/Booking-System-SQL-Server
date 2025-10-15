CREATE OR ALTER FUNCTION fn_ListRequestedSupportByHotel
(
	@HotelID INT = 1
)
RETURNS TABLE
AS
RETURN
(
	SELECT
		COUNT(s.ID) AS 'Requests',
		u.Name,
		u.Email,
		MAX(s.CreatedDate) AS 'Last Message'
	FROM Users u
		INNER JOIN SupportMessages s
	ON 
		s.UserID = u.ID  
	WHERE
		s.HotelID = @HotelID
	GROUP BY
		u.ID,
		u.Name,
		u.Email
);
GO