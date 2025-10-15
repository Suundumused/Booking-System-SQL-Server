BEGIN
	-- Bookings

	CREATE INDEX IX_Bookings_HotelID_CheckInOut
		ON Bookings (HotelID, CheckIn, CheckOut)
	INCLUDE (ID, UserID);

	-- Calendar Blocks

	CREATE INDEX IX_CalendarBlocks_HotelID_DateInOut
		ON CalendarBlocks (HotelID, DateIn, DateOut)
	INCLUDE (ID);

	-- Custom Prices

	CREATE INDEX IX_CustomPrices_HotelID_DateInOut
		ON CustomPrices (HotelID, DateIn, DateOut)
	INCLUDE (ID, Price);

	-- Support Messages
	CREATE INDEX IX_SupportMessages_UserID
		ON SupportMessages (UserID)
	INCLUDE (ID, HotelID, CreatedDate);
END
GO;