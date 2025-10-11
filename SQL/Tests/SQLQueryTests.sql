INSERT INTO Users (Username, PasswordHash, Salt, Email) VALUES ('jao123', '23hfuwfeqwiu', 'eueqwuiqwrh9842', 'joao123@gmail.com');
INSERT INTO Users (Username, PasswordHash, Salt, Email) VALUES ('carrada', '23hfuwfeqwiu', 'eueqwuiqwrh9842', 'carrada@gmail.com');
INSERT INTO Users (Username, PasswordHash, Salt, Email) VALUES ('deca', '23hfuwfeqwiu', 'eueqwuiqwrh9842', 'deca@gmail.com');
INSERT INTO Users (Username, PasswordHash, Salt, Email) VALUES ('joca5', '23hfuwfeqwiu', 'eueqwuiqwrh9842', 'joca5@hotmail.com');
INSERT INTO Users (Username, PasswordHash, Salt, Email) VALUES ('jussarada', '234452fegwd', 'wedrwd', 'jussarada@gmail.com');

INSERT INTO Hotels (Name, Location) VALUES ('Aurélio Json HH', 'Street Catumbi 1230, Rio De Janeiro');
INSERT INTO Hotels (Name, Location) VALUES ('White House', 'Street 345, Bogotá');

INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('03-05-2025 10:00:00.000', '08-05-2025 14:30:00.005', 1, 2);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2025 10:00:00.000', '12-07-2025 14:30:00.005', 1, 2);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2025 10:00:00.000', '12-07-2025 14:30:00.005', 1, 3);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2026 10:00:00.000', '11-07-2026 14:30:00.005', 2, 4);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('11-06-2026 10:00:00.000', '11-07-2026 14:30:00.005', 2, 4);
INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('09-01-2026 10:00:00.000', '09-03-2026 14:30:00.005', 1, 5);


ALTER TABLE Users
	ADD CONSTRAINT UQ_Users_Email UNIQUE (Email);

ALTER TABLE Bookings
	DROP CONSTRAINT PK__Bookings__01461D2D5D7394D1;

ALTER TABLE Bookings
	ADD ID INT IDENTITY PRIMARY KEY;


ALTER TABLE CalendarBlocks
	DROP CONSTRAINT PK__Calendar__A0608191E40C26C6;

ALTER TABLE CalendarBlocks
	ADD ID INT IDENTITY PRIMARY KEY;


SELECT * FROM Users;
SELECT * FROM Hotels;
SELECT * FROM Bookings;

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