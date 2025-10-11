import pyodbc

from random import randint


CONNECTION_STRING = (
    "Driver={ODBC Driver 17 for SQL Server};"
    "Server=DESKTOP-NLHVPUD;"
    "Database=HotelNetworkDB;"
    "Trusted_Connection=yes;"
)


cnxn = pyodbc.connect(CONNECTION_STRING)
cursor = cnxn.cursor()
    
for i in range(0, 800):
    cursor.execute(f"INSERT INTO Bookings (CheckIn, CheckOut, HotelID, UserID) VALUES ('{randint(1, 12)}-{randint(1, 28)}-20{randint(25, 27)} 10:00:00.000', '{randint(1, 12)}-{randint(1, 28)}-20{randint(26, 27)} 14:30:00.005', {randint(1, 2)}, {randint(1, 5)})")
    cnxn.commit()