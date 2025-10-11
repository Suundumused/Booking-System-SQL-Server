BEGIN
	CREATE TABLE Users(
		ID INT IDENTITY PRIMARY KEY,

		Username VARCHAR(100) UNIQUE NOT NULL,
		Email VARCHAR(100) UNIQUE NOT NULL,
		PasswordHash VARCHAR(256) NOT NULL,
		Salt VARCHAR(256) NOT NULL,

		Name VARCHAR(100),
		Phone1 VARCHAR(19),
		Phone2 VARCHAR(19),

		CreationDate DATETIME DEFAULT GETDATE(),
		LastLoginDate DATETIME
	);

	CREATE TABLE Hotels(
		ID INT IDENTITY PRIMARY KEY,
		Name VARCHAR(150),
		Location VARCHAR(255)
	);

	CREATE TABLE Bookings(
		ID INT IDENTITY PRIMARY KEY,

		CheckIn DATETIME NOT NULL,
		CheckOut DATETIME NOT NULL,

		UserID INT NOT NULL,
		HotelID INT NOT NULL,
		
		CONSTRAINT FK_Bookings_Hotels FOREIGN KEY (HotelID) REFERENCES Hotels(ID),
		CONSTRAINT FK_Bookings_Users FOREIGN KEY (UserID) REFERENCES Users(ID)
	);

	CREATE INDEX IX_Bookings ON Bookings (UserID, HotelID) INCLUDE (CheckIn, CheckOut);
	CREATE INDEX IX_Bookings_UserID ON Bookings (UserID) INCLUDE (CheckIn, CheckOut, HotelID);
	CREATE INDEX IX_Bookings_HotelID ON Bookings (HotelID) INCLUDE (CheckIn, CheckOut, UserID);
	CREATE INDEX IX_Bookings_Check_InOut ON Bookings (CheckIn, CheckOut) INCLUDE (UserID, HotelID);

	CREATE TABLE CalendarBlocks(
		ID INT IDENTITY PRIMARY KEY,

		DateIn DATETIME NOT NULL,
		DateOut DATETIME NOT NULL,

		HotelID INT NOT NULL,

		CONSTRAINT FK_CalendarBlocks_Hotels FOREIGN KEY (HotelID) REFERENCES Hotels(ID)
	);

	CREATE INDEX IX_CalendarBlocks_HotelID ON CalendarBlocks(HotelID) INCLUDE (DateIn, DateOut);
	CREATE INDEX IX_CalendarBlocks_DateInOut ON CalendarBlocks(DateIn, DateOut) INCLUDE (HotelID);

	CREATE TABLE SupportMessages(
		ID INT IDENTITY PRIMARY KEY,

		UserID INT NOT NULL,
		HotelID INT NOT NULL,

		Message VARCHAR(500),

		CreatedDate DATETIME DEFAULT GETDATE(),

		CONSTRAINT FK_SupportMessages_Users FOREIGN KEY (UserID) REFERENCES Users(ID),
		CONSTRAINT FK_SupportMessages_Hotels FOREIGN KEY (HotelID) REFERENCES Hotels(ID)
	);

	CREATE INDEX IX_SupportMessages ON SupportMessages(UserID, HotelID) INCLUDE (Message, CreatedDate);
	CREATE INDEX IX_SupportMessages_UserID ON SupportMessages(UserID) INCLUDE (HotelID, Message, CreatedDate);
	CREATE INDEX IX_SupportMessages_HotelID ON SupportMessages(HotelID) INCLUDE (UserID, Message, CreatedDate);
END;
GO