# SQL Project - Hotel Network

> **Summary:** Complete documentation for the SQL database project (schema, objects, triggers, functions, procedures, indexes, naming conventions, deployment and tests).

## Table of Contents

1. [Overview](#overview)
2. [Project Files](#project-files)
3. [Prerequisites](#prerequisites)
4. [Data Model - Core Tables](#data-model---core-tables)
5. [Recommended Indexes](#recommended-indexes)
6. [User-Defined Functions (UDFs)](#user-defined-functions-udfs)
7. [Stored Procedures & Utilities](#stored-procedures--utilities)
8. [Triggers](#triggers)
9. [Usage Examples (queries)](#usage-examples-queries)
10. [Validation & Recommended Tests](#validation--recommended-tests)
11. [Deployment / Execution Order](#deployment--execution-order)
12. [Security & Permissions](#security--permissions)
13. [Error Messages & Handling](#error-messages--handling)

## Overview

This project implements a hotel booking system in T-SQL with the following features:

- Independent reservations, prices, support messages, and calendar for each hotel.
- User Registration and Login "Password / Salt"
- Creation of reservations by managing the number of rooms available in relation to reservation overlapping dates and/or blocked dates.
- The same applies to blackout dates on the calendar and customized daily prices.
- Blocking date ranges via `CalendarBlocks`
- Custom daily prices via `CustomPrices`
- Automatic final price calculation (combining hotel base price and custom daily prices)
- Reusable functions for overlap checks, user and hotel reports
- Validation of incorrect entry and exit dates.

The goal is to enforce business integrity at the database level and provide reusable query objects for the application layer.

## Project Files

Files included in the repository (original names):

- `DataBase.sql` - Initial configuration (schemas, server settings)
- `Tables.sql` - Table creation scripts (Bookings, Hotels, Users, CustomPrices, CalendarBlocks and SupportMessages)
- `Indexes.sql` - Index creation scripts
- `fn_ListBookingsByHotel.sql` - Function to list bookings by hotel
- `fn_ListBookingsByUser.sql` - Function to list bookings by user
- `fn_ListRequestedSupportByHotel.sql` - Function that aggregates support requests by hotel
- `usp_DateTimeRangeChecker.sql` - Utility stored procedure for date range checks
- `trg_Check_Booking_Overlaps.sql` - `INSTEAD OF INSERT` trigger to validate Bookings
- `trg_Check_CalendarBlocks_Overlaps.sql` - Trigger for CalendarBlocks validation
- `trg_Check_CustomPrices_Overlaps.sql` - Trigger for CustomPrices validation

## Prerequisites

- Microsoft SQL Server 2016+ (SQL Server 2017/2019/2022 recommended). `CREATE OR ALTER` requires a compatible version.
- DDL/DML permissions to create objects in the target schema (typically `dbo`).
- Optional: `tSQLt` for unit testing if you want automated tests.

## Data Model - Core Tables

This section describes the primary tables used by the project. Refer to `Tables.sql` for the exact column definitions and constraints.

### `Hotels`
- `ID INT PK`
- `Name NVARCHAR(200)`
- `Location NVARCHAR(255)`
- `Price DECIMAL(10,2)` - base daily price.
- `Rooms INT` - Total rooms available on overlapping dates.

### `Bookings`
- `ID INT PK` (IDENTITY)
- `UserID INT` (FK → Users.ID)
- `HotelID INT` (FK → Hotels.ID)
- `CheckIn DATETIME2`
- `CheckOut DATETIME2`
- `Price DECIMAL(18,2)` - Final booking price.
- `CreatedAt DATETIME2` (default `SYSUTCDATETIME()`)

### `CustomPrices`
- `ID INT PK`
- `HotelID INT` (FK)
- `DateIn DATETIME2`
- `DateOut DATETIME2`
- `Price DECIMAL(10,2)` - Daily price.

### `CalendarBlocks`
- `ID INT PK`
- `HotelID INT` (FK)
- `DateIn DATETIME2`
- `DateOut DATETIME2`
- `Reason NVARCHAR(255)`

### `Users`, `SupportMessages`
- `Users(ID, Username, Email, Name, ...)`
- `SupportMessages(ID, UserID, HotelID, Message, CreatedDate)`

## Recommended Indexes

Booking overlap queries are performance-sensitive. Recommended index:

```sql
CREATE INDEX IX_Bookings_Hotel_DateRange
  ON Bookings (HotelID, CheckIn, CheckOut)
INCLUDE (ID);
```

Why:
- `HotelID` is equality-filtered - make it the first key column.
- `CheckIn`/`CheckOut` are range filters and help reduce the scanned rows.
- `INCLUDE (ID)` avoids lookups for `COUNT(ID)` queries.

Other useful indexes:
- `IX_CustomPrices_Hotel_DateRange (HotelID, DateIn, DateOut)`
- `IX_CalendarBlocks_Hotel_DateRange (HotelID, DateIn, DateOut)`
- `IX_Users_Email` 
- `IX_Users_Username`

Always keep statistics up-to-date (`UPDATE STATISTICS`) after bulk loads.

## User-Defined Functions (UDFs)

Files provided and their purpose:

- `fn_ListBookingsByHotel.sql` - Returns bookings for a given hotel (optional date filters).
- `fn_ListBookingsByUser.sql` - Returns bookings for a specific user.
- `fn_ListRequestedSupportByHotel.sql` - Aggregates support requests per user with last message.

Notes:
- Use `DATETIME2` for better precision.
- Keep column names explicit in `RETURNS TABLE` block.

## Stored Procedures & Utilities

- `usp_DateTimeRangeChecker.sql` - Utility procedure for checking date-time intersections (useful for tests and callers outside triggers).

## Triggers

Main trigger: `trg_Check_Booking_Overlaps` - `INSTEAD OF INSERT` on `Bookings` that enforces:
- `CheckIn < CheckOut` validation per row.
- `CalendarBlocks` overlap check (abort if any inserted row intersects a block).
- Capacity check (`Rooms`) combining existing overlaps and other rows in the same batch.
- Final price computation per inserted row (custom price days + remaining days × hotel base price).

Final price calculation (per row):
- For each `CustomPrices` entry that intersects the booking, compute `overlapDays * cp.Price` and sum.
- If booking has an explicit `Price` provided on insert, use it.
- Otherwise: `final = sumCustomPrice + (bookingDays - overlapDays) * hotelBasePrice`.

---

## Usage Examples (queries)

**Count overlaps for hotel in date range:**
```sql
DECLARE @Start DATETIME2 = '2025-03-01 00:00:00', @End DATETIME2 = '2025-03-09 23:59:59';
SELECT COUNT(ID) AS OverlapCount
FROM Bookings
WHERE HotelID = 1
  AND CheckIn < @End
  AND CheckOut > @Start;
```

**Insert booking example (trigger performs validation & price):**
```sql
INSERT INTO Bookings (CheckIn, CheckOut, UserID, HotelID)
VALUES ('2025-07-01', '2025-07-05', 10, 1);
```

---

## Validation & Recommended Tests

Test list:
1. Single-row insert: valid booking inserts successfully.
2. Multi-row batch insert: valid bookings all insert successfully.
3. Batch insert exceeding `Rooms` capacity: whole batch is rejected.
4. Insert where `CheckIn >= CheckOut`: rejected.
5. Insert intersecting `CalendarBlocks`: rejected.
6. Booking crossing `CustomPrices` periods: price is calculated correctly per day.
7. Concurrency test: two concurrent sessions inserting for the same hotel - verify no race conditions.

Test tools and hints:
- Use `BEGIN TRAN / ROLLBACK` to run non-destructive tests.
- Use `SET STATISTICS IO, TIME ON` to measure IO and CPU cost.
- Use `SQL Server Profiler` / `Extended Events` to detect blocking.

## Deployment / Execution Order

1. `DataBase.sql` (schemas, server-level setup)
2. `Tables.sql` (create tables & constraints)
3. `Indexes.sql` (create indexes - create non-clustered indexes after heavy seed load if needed)
4. `fn_*.sql` (functions)
5. `usp_*.sql` (procedures)
6. `trg_*.sql` (triggers) - create triggers last or disable them during seed load
7. Seed data / migrations

## Security & Permissions

- Create roles like `booking_app_read` and `booking_app_writer` with minimal privileges.
- Grant `EXECUTE` on procedures/functions where appropriate.
- Be cautious with ownership chaining and cross-database access in triggers.

## Error Messages & Handling

Standard trigger error pattern used in this project:
```sql
THROW 51000, 'Friendly error message here', 1;
```
Common messages:
- `The entry date must be earlier than the exit date.`
- `Max rooms exhausted for the selected hotel.`
- `The reservation dates overlap with blocking dates.`

Document error messages and codes so the application layer can present meaningful information to users.

## Performance & Concurrency Checklist

- Index: `Bookings(HotelID, CheckIn, CheckOut)`
- Index: `CustomPrices(HotelID, DateIn, DateOut)`
- Consider `sp_getapplock` by `HotelID` or `SERIALIZABLE` isolation for critical sections
- Monitor `PAGE LATCH` and blocking at peak times
- Use filtered indexes for active rows if you have a `Cancelled` or `IsActive` flag