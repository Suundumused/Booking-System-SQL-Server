INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '2025-09-10 00:00:00.001', '2025-09-30 23:59:59.000');
INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '2025-09-02 00:00:00.001', '2025-09-08 23:59:59.000');

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-10-06 00:00:00.001', '2025-10-09 23:59:59.000', 2, 1);

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-12-03 00:00:00.001', '2025-12-09 23:59:59.000', 2, 1);
INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-05-03 00:00:00.001', '2025-05-09 23:59:59.000', 2, 1);

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-12-02 00:00:00.001', '2025-12-07 23:59:59.001', 170.50);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-05-02 00:00:00.001', '2025-05-10 23:59:59.001', 100.00);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-03-02 00:00:00.001', '2025-03-04 23:59:59.001', 100.00);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-03-05 00:00:00.001', '2025-03-07 23:59:59.001', 50.00);

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-03-01 00:00:00.001', '2025-03-09 23:59:59.000', 2, 1);

INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (3, 2, 'Test 0');
INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (1, 2, 'Test 1');

SELECT * FROM Users;
SELECT * FROM Hotels;
SELECT * FROM Bookings;
SELECT * FROM CalendarBlocks;
SELECT * FROM CustomPrices;
SELECT * FROM SupportMessages;

SELECT 1
	FROM CustomPrices
WHERE
	HotelID = 3
AND
	DateIn < 'sadsadsasa'
AND
	DateOut > '12312f'

SELECT * FROM fn_ListBookingsByUser(DEFAULT, 'Mariatalba');
SELECT * FROM fn_ListBookingsByHotel(1);
SELECT * FROM fn_ListRequestedSupportByHotel(2);

DECLARE @ID INT;
EXEC @ID = dbo.fn_Login 'fno3n9p8fhj', 'efwh982f92';
PRINT(@ID);

SELECT * FROM Bookings WHERE UserID = 2;
SELECT * FROM Bookings WHERE HotelID = 2;

SELECT * FROM CustomPrices WHERE HotelID = 2;
SELECT * FROM CalendarBlocks WHERE HotelID = 2;
SELECT * FROM SupportMessages WHERE HotelID = 2;

SELECT * 
	FROM Bookings b 
WHERE 
	UserID = 2 
AND 
	HotelID = 2 
AND 
	CheckIn > 'xxx' 
AND 
	CheckOut < 'xxx';