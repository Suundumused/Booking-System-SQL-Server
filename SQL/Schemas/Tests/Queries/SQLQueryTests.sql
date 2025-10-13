INSERT INTO Users (Username, Email, PasswordHash, Salt, Name, Phone1) VALUES ('Joaopipa123', 'jambofruta@gmail.com', 'fno3n9p8fhj', 'efwh982f92', 'Joao', '22659261');
INSERT INTO Users (Username, Email, PasswordHash, Salt, Name, Phone1) VALUES ('Mariatalba', 'tabata@wmail.com', 'n4453n9p8fhj', 'ef234g982f92', 'Maria', '23245679');
INSERT INTO Users (Username, Email, PasswordHash, Salt, Name, Phone1) VALUES ('Ratao', 'ratata@wmail.com', 'g4344yt32', '2f42f3w43', 'Ratinho', '12345678');

INSERT INTO Hotels (Name, Location, Rooms, Price) VALUES ('Aurélio Json HH', 'Street Catumbi 1230, Rio De Janeiro', 3, 360.00);
INSERT INTO Hotels (Name, Location, Rooms, Price) VALUES ('White House', 'Street 345, Bogotá', 4, 420.00);

INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '03-05-2025 00:00:00.001', '03-15-2025 23:59:00.001');
INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (1, '09-06-2025 00:00:00.001', '09-29-2025 23:59:00.001');
INSERT INTO CalendarBlocks (HotelID, DateIn, DateOut) VALUES (2, '01-03-2026 00:00:00.001', '01-07-2025 23:59:00.001');

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '11-06-2025 07:00:00.000', '11-18-2025 10:00:00.000', 520.65);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (1, '11-06-2025 07:00:00.000', '11-18-2025 10:00:00.000', 750.30);

INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (2, '11-06-2025 07:00:00.000', '11-18-2025 10:00:00.000', 600.65);
INSERT INTO CustomPrices (HotelID, DateIn, DateOut, Price) VALUES (2, '12-25-2025 07:00:00.000', '01-01-2026 10:00:00.000', 950.30);

INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('03-05-2025 10:00:00.000', '08-05-2025 14:30:00.005', 1, 2);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2025 10:00:00.000', '12-07-2025 14:30:00.005', 1, 2);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2025 10:00:00.000', '12-07-2025 14:30:00.005', 1, 3);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2026 10:00:00.000', '11-07-2026 14:30:00.005', 2, 4);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2026 10:00:00.000', '11-07-2026 14:30:00.005', 2, 4);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('09-01-2026 10:00:00.000', '09-03-2026 14:30:00.005', 1, 5);

INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID) VALUES ('09-03-2026 10:00:00.000', '09-04-2026 14:30:00.005', 5, 4);

INSERT INTO CalendarBlocks (DateIn, DateOut, HotelID) VALUES ('09-02-2026 10:11:00', '09-02-2026 19:35:00.006', 4);
INSERT INTO CalendarBlocks (DateIn, DateOut, HotelID) VALUES ('10-02-2026 10:11:00', '10-03-2026 19:35:00.006', 4);

DELETE FROM Bookings WHERE ID = 8;
DELETE FROM CalendarBlocks WHERE ID = 1;


ALTER TABLE Users
	ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);

ALTER TABLE Users
    ADD CONSTRAINT CK_Users_Name CHECK (LEN(Name) > 2)

UPDATE Users SET Name = '"NOT SET"' WHERE Name IS NULL;

ALTER TABLE Users
    ALTER COLUMN Name VARCHAR(100) NOT NULL;

ALTER TABLE Bookings
	DROP CONSTRAINT PK__Bookings__01461D2D5D7394D1;

ALTER TABLE Bookings
	ADD ID INT IDENTITY PRIMARY KEY;

ALTER TABLE Bookings
	ALTER COLUMN Price DECIMAL(10, 2);

ALTER TABLE Hotels
	ADD Price DECIMAL(10, 2);

ALTER TABLE CustomPrices
    ALTER COLUMN Price DECIMAL(10, 2) NOT NULL;

UPDATE Hotels SET Price = 420.00 WHERE ID = 4;

ALTER TABLE CalendarBlocks
	DROP CONSTRAINT PK__Calendar__A0608191E40C26C6;

ALTER TABLE CalendarBlocks
	ADD ID INT IDENTITY PRIMARY KEY;

ALTER TABLE Bookings
    ADD Price DECIMAL(10, 2) NOT NULL;

UPDATE Hotels SET Rooms = 4 WHERE ID = 3;
UPDATE Hotels SET Rooms = 4 WHERE ID = 4;

SELECT * FROM Users;
SELECT * FROM Hotels;
SELECT * FROM Bookings;
SELECT * FROM CalendarBlocks;
SELECT * FROM CustomPrices;
SELECT * FROM SupportMessages;

SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;

SELECT u.Username, b.CheckIn, h.Name as 'Hotel Name' FROM Bookings b RIGHT JOIN Users u ON b.UserID = u.ID LEFT JOIN Hotels h On b.HotelID = h.ID;

CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Bookings_UserID_HotelID ON Bookings(UserID) INCLUDE (CheckIN);
CREATE INDEX IX_Hotels_ID ON Hotels (ID) INCLUDE (Name);

DROP INDEX IX_Bookings_CheckIn ON Bookings;


SELECT u.Username, b.CheckIn FROM Bookings b INNER JOIN Users u ON b.UserID = u.ID;

SELECT u.Username, b.CheckIn FROM Users u INNER JOIN Bookings b ON u.ID = b.UserID;

SELECT * FROM Bookings b WHERE b.UserID = 3;

SELECT * FROM Users WHERE Email = 'jussarada@gmail.com';

SELECT
    OBJECT_NAME(s.object_id) AS TableName,
    i.name AS IndexName,
    s.user_seeks,
    s.user_scans,
    s.user_lookups,
    s.user_updates,
    s.last_user_seek,
    s.last_user_scan,
    s.last_user_lookup,
    s.last_user_update
FROM 
    sys.dm_db_index_usage_stats s
JOIN 
    sys.indexes i ON s.object_id = i.object_id AND s.index_id = i.index_id
WHERE 
    OBJECTPROPERTY(s.object_id, 'IsUserTable') = 1 -- Filter for user tables
ORDER BY 
    TableName, IndexName;

SELECT DateIn, DateOut
FROM CalendarBlocks
WHERE HotelID = 1;



SELECT * FROM VW_Bookings_Users_Hotel_CalendarBlocks;

SELECT * FROM UDF_User_Hotel_Bookings('jussarada@gmail.com', 3);

DELETE FROM Users;
DELETE FROM Bookings;
DELETE FROM Hotels;
DELETE FROM CalendarBlocks;
DELETE FROM CustomPrices;
DELETE FROM SupportMessages;