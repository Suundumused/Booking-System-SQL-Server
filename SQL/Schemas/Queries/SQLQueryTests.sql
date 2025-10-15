SELECT * FROM Users;
SELECT * FROM Hotels;
SELECT * FROM Bookings;
SELECT * FROM CalendarBlocks;
SELECT * FROM CustomPrices;
SELECT * FROM SupportMessages;

-- ano / mes / dia


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

INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (3, 2, 'N�o consigo fazer a reserva');
INSERT INTO SupportMessages (UserID, HotelID, Message) VALUES (1, 2, 'N�o consigo ddddddddddddd a reserva');


DELETE FROM Bookings WHERE ID > 1;
DELETE FROM CustomPrices;

ALTER TABLE SupportMessages
	ALTER COLUMN Message NVARCHAR(500) NOT NULL;

ALTER TABLE SupportMessages
	ADD CONSTRAINT CK_Len_Message CHECK (LEN(Message) > 0);


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