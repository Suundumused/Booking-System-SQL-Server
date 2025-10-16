BEGIN
	-- Users

	CREATE INDEX IX_Users_Login
		ON Users (PasswordHash, Salt)
	INCLUDE (ID, Username, Email, Name, Phone1, Phone2, CreationDate, LastLoginDate);
	
	-- Bookings
	
	CREATE INDEX IX_Bookings_UserID
		ON Bookings (UserID)
	INCLUDE (ID, CheckIn, CheckOut, HotelID, Price);

	CREATE INDEX IX_Bookings_HotelID
		ON Bookings (HotelID)
	INCLUDE (ID, CheckIn, CheckOut, UserID, Price);

	CREATE INDEX IX_Bookings_HotelID_CheckInOut
		ON Bookings (HotelID, CheckIn, CheckOut)
	INCLUDE (ID, UserID);

	-- Calendar Blocks

	CREATE INDEX IX_CalendarBlocks_HotelID
		ON CalendarBlocks (HotelID)
	INCLUDE (ID, DateIn, DateOut);

	CREATE INDEX IX_CalendarBlocks_HotelID_DateInOut
		ON CalendarBlocks (HotelID, DateIn, DateOut)
	INCLUDE (ID);

	-- Custom Prices

	CREATE INDEX IX_CustomPrices_HotelID
		ON CustomPrices (HotelID)
	INCLUDE (ID, DateIn, DateOut, Price);

	CREATE INDEX IX_CustomPrices_HotelID_DateInOut
		ON CustomPrices (HotelID, DateIn, DateOut)
	INCLUDE (ID, Price);

	-- Support Messages

	CREATE INDEX IX_SupportMessages_HotelID
		ON SupportMessages (HotelID)
	INCLUDE (ID, UserID, CreatedDate);

	CREATE INDEX IX_SupportMessages_UserID
		ON SupportMessages (UserID)
	INCLUDE (ID, HotelID, CreatedDate);
END
GO;