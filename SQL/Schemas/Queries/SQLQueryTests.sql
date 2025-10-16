SELECT * FROM Users;
SELECT * FROM Hotels;
SELECT * FROM Bookings;
SELECT * FROM CalendarBlocks;
SELECT * FROM CustomPrices;
SELECT * FROM SupportMessages;


INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '2025-09-10 00:00:00.001', '2025-09-30 23:59:59.000');
INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '2025-09-02 00:00:00.001', '2025-09-08 23:59:59.000');

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-12-02 00:00:00.001', '2025-12-07 23:59:59.001', 170.50);

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-10-06 00:00:00.001', '2025-10-09 23:59:59.000', 2, 1);
INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-12-03 00:00:00.001', '2025-12-09 23:59:59.000', 2, 1);

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-05-02 00:00:00.001', '2025-05-10 23:59:59.001', 100.00);
INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-05-03 00:00:00.001', '2025-05-09 23:59:59.000', 2, 1);

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-03-02 00:00:00.001', '2025-03-04 23:59:59.001', 100.00);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '2025-03-05 00:00:00.001', '2025-03-07 23:59:59.001', 50.00);

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('2025-03-01 00:00:00.001', '2025-03-09 23:59:59.000', 2, 1);

INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (3, 2, 'Não consigo fazer a reserva');
INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (1, 2, 'Não consigo ddddddddddddd a reserva');

DELETE FROM Bookings WHERE ID > 1;
DELETE FROM CustomPrices;

ALTER TABLE SupportMessages
	ALTER COLUMN Message NVARCHAR(500) NOT NULL;

ALTER TABLE SupportMessages
	ADD CONSTRAINT CK_Len_Message CHECK (LEN(Message) > 0);

ALTER TABLE Hotels
	ALTER COLUMN Location NVARCHAR(255) NOT NULL;

ALTER TABLE Users
	ADD CONSTRAINT UQ_PasswordHash UNIQUE (PasswordHash);

ALTER TABLE Users
	ADD CONSTRAINT UQ_PasswordSalt UNIQUE (Salt);

SELECT
	Price,
	DATEDIFF(
		DAY,
		CASE WHEN DateIn < 'ASDSADSAAS' THEN 'SADSADSA' ELSE DateIn END,
		CASE WHEN DateOut > 'SADASDADSA' THEN 'SADASDSA' ELSE DateOut END
	)
FROM CustomPrices
WHERE
	HotelID = 2
AND
	DateIn < 'ASDSADSASA'
AND
	DateOut > 'QWEQEWQEWQWQ';

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

SELECT * FROM Bookings WHERE UserID = 2;
SELECT * FROM Bookings WHERE HotelID = 2;

SELECT * FROM CustomPrices WHERE HotelID = 2;
SELECT * FROM CalendarBlocks WHERE HotelID = 2;

SELECT * FROM SupportMessages WHERE HotelID = 2;

SELECT * FROM Bookings b WHERE UserID = 2 AND HotelID = 2 AND CheckIn > 'xxx' AND CheckOut < 'xxx';

DECLARE @Start DATETIME2 = '2025-03-01 00:00:00', @End DATETIME2 = '2025-03-09 23:59:59';
SELECT COUNT(ID) AS OverlapCount
FROM Bookings
WHERE HotelID = 1
  AND CheckIn < @End
  AND CheckOut > @Start;

DECLARE @UserID INT = (SELECT ID FROM Users WHERE PasswordHash = 'iewf292981nfn223t24g' AND Salt = '29h8ifh8');

DECLARE @ID INT = (SELECT 
	ID
FROM 
	Users
WHERE PasswordHash = 'fno3n9p8fhj' AND Salt = 'efwh982f92');

UPDATE Users
	SET LastLoginDate = GETDATE()
WHERE ID = @ID

PRINT(@ID);