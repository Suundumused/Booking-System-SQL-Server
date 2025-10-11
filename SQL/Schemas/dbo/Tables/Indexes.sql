CREATE INDEX IX_Bookings ON Bookings (UserID, HotelID) INCLUDE (CheckIn, CheckOut);
CREATE INDEX IX_Bookings_UserID ON Bookings (UserID) INCLUDE (CheckIn, CheckOut, HotelID);
CREATE INDEX IX_Bookings_HotelID ON Bookings (HotelID) INCLUDE (CheckIn, CheckOut, UserID);
CREATE INDEX IX_Bookings_Check_InOut ON Bookings (CheckIn, CheckOut) INCLUDE (UserID, HotelID);

CREATE INDEX IX_CalendarBlocks_HotelID ON CalendarBlocks(HotelID) INCLUDE (DateIn, DateOut);
CREATE INDEX IX_CalendarBlocks_DateInOut ON CalendarBlocks(DateIn, DateOut) INCLUDE (HotelID);

CREATE INDEX IX_SupportMessages ON SupportMessages(UserID, HotelID) INCLUDE (Message, CreatedDate);
CREATE INDEX IX_SupportMessages_UserID ON SupportMessages(UserID) INCLUDE (HotelID, Message, CreatedDate);
CREATE INDEX IX_SupportMessages_HotelID ON SupportMessages(HotelID) INCLUDE (UserID, Message, CreatedDate);