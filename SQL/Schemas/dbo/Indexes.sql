BEGIN
	--Bookings
	CREATE INDEX IX_Bookings_HotelID 
		ON Bookings (HotelID) 
	INCLUDE (CheckIn, CheckOut);

	CREATE INDEX IX_Bookings_HotelID_CheckInOut 
		ON Bookings (HotelID, CheckIn, CheckOut)
	INCLUDE (UserID);

	--Users
	CREATE INDEX IX_Users_Email
		ON Users (Email)
	INCLUDE (Name, Phone1, Phone2);

	--Calendar Blocks

	CREATE INDEX IX_CalendarBlocks_HotelID 
		ON CalendarBlocks (HotelID) 
	INCLUDE (DateIn, DateOut);

	CREATE INDEX IX_CalendarBlocks_HotelID_DateInOut 
		ON CalendarBlocks (HotelID, DateIn, DateOut);

	--Support Messages

	CREATE INDEX IX_SupportMessages 
		ON SupportMessages(UserID, HotelID) 
	INCLUDE (Message, CreatedDate);

	CREATE INDEX IX_SupportMessages_UserID 
		ON SupportMessages(UserID) 
	INCLUDE (HotelID, Message, CreatedDate);

	CREATE INDEX IX_SupportMessages_HotelID 
		ON SupportMessages(HotelID) 
	INCLUDE (UserID, Message, CreatedDate);
END;
GO


