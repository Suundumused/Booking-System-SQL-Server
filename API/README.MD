# ZConnector ASP.NET API

Robust and secure API, asynchronously implements the functionalities of client-side communication with the SQL Server for registration, login, profile, room management, prices, dates and bookings per hotel.

## Overview

This project serves as a base for building modern web APIs using:
- **ASP.NET Core 8.0+**
- **Entity Framework Core**
- **AutoMapper**
- **Asynchronous Programming**
- **Global Exception handling**
- **Global Exception handling for Data Annotations**
- **Capture Custom Trigger/Procedures/Functions exceptions**
- **Register and Login** `Bearer Token`
- **Update user profile**
- **Booking System per hotel.**
- **Room management per hotel.**
- **Customized price by date range.**
- **Custom blocked dates.**
- **HTTPS security Headers**
- **HTTPS only protocol**
- **Handled Triggers**
- **Claims for internal user identification.**
- **End-Point Data security validation for the user**

## Usage

    dotnet restore

### appsettings.json

- Replace the `DbConnection` in `ConnectionStrings` with your own SQL Connection.
- Run `GenerateToken.py` copy the token and update at `JWT` in `Secret` to the one you generated.
    
## Routes

    dotnet run

<table style="width: 100%; border-collapse: collapse; ">
    <tr>
        <th style="text-align: center;">
Route
        </th>
        <th style="text-align: center;">
Method
        </th>
        <th style="text-align: center;">
Description
        </th>
        <th style="text-align: center; width: 50%;">
Usage
        </th>
    </tr>
    <tr>
        <td style="text-align: center;">
api/auth/login
        </td>
        <td style="text-align: center;">
POST
        </td>
        <td>
Returns User profile and auth Token
        </td>
        <td>
            <pre>
json
{
    "email" : "cavalgador21@gmail.com",
    "Password" : "jaguara"
}
            </pre>
        </td>
    </tr>
        <tr>
        <td style="text-align: center;">
api/auth/register
        </td>
        <td style="text-align: center;">
POST
        </td>
        <td>
Password is saved as Hash and Salt on the SQL Server.
        </td>
        <td>
            <pre>
json
{
    "userName": "cavalo123",
    "email": "cavalgador21@gmail.com",
    "password": "jaguara",
    "Name": "Cavalo"
}
            </pre>
        </td>
    </tr>
    </tr>
    <tr>
        <td style="text-align: center;">
api/profile/data
        </td>
        <td style="text-align: center;">
GET
        </td>
        <td>
Return user profile data
        </td>
        <td>
            <pre>
returns json
{
    "id": 2,
    "username": "cavalo123",
    "email": "cavalgador21@gmail.com",
    "name": "Cavalo Alterado",
    "phone1": null,
    "phone2": null,
    "lastLoginDate": "2025-10-27T15:29:29.79"
}
            </pre>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
api/profile/update
        </td>
        <td style="text-align: center;">
PATCH
        </td>
        <td>
Updates one or more fields in the user profile, except credentials.
        </td>
        <td>
            <pre>
json
{
    "Name": "Cavalo Alterado"
}
            </pre>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
api/booking/{hotelId}
        </td>
        <td style="text-align: center;">
GET
        </td>
        <td>
Lists all of the user's bookins at a given hotel.
        </td>
        <td>
            <pre>
json
[
    {
        "id": 6,
        "checkIn": "2025-12-28T00:00:00",
        "checkOut": "2026-01-06T00:00:00",
        "userID": 2,
        "user": null,
        "userModel": null,
        "hotelID": 1,
        "hotel": null,
        "price": 3098.88
    }
]
            </pre>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
api/booking/new
        </td>
        <td style="text-align: center;">
POST
        </td>
        <td>
New booking.
        </td>
        <td>
            <pre>
json
{
    "checkIn": "2026-03-21",
    "checkOut": "2026-03-28",
    "hotelID": 1 
}
            </pre>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
api/booking/update
        </td>
        <td style="text-align: center;">
PATCH
        </td>
        <td>
Update booking.
        </td>
        <td>
            <pre>
json
{
    "id": 6,
    "checkOut": "2026-01-06"
}
            </pre>
        </td>
    </tr>
    <tr>
        <td style="text-align: center;">
api/booking/{bookingId}
        </td>
        <td style="text-align: center;">
DELETE
        </td>
        <td>
Delete booking.
        </td>
        <td>
        </td>
    </tr>
</table>

### Common responses

- `200`
- `204` - Empty Array
- `400` - A required field for {objectName} is missing.
- `401` - ... already exists.
- `404` - Object not found.
- `409` - The date block overlaps an existing booking.
- `529` - Max rooms exhausted for the selected hotel.
- `400` - The exit Time and Date must be later than the entry date.
- `500` - An unexpected database error occurred. Target is {objectName}.
- ContentType = `text/plain`

## Extra Features

- `ForwardedHeadersMiddleware` - Allows the application to correctly identify the original client's IP address, requested host, and protocol (HTTP/HTTPS).
- Model State Filter is Managed in `GlobalEndPointHandler.cs`
- `ApiEfCoreHandler.cs` manages custom error codes from the SQL database and provides custom `EfSafeException` exception messages.
- `Auto Mapping` intertwines models and entities between client and database.
- `OnAuthenticationFailed` - JWT returns `text/plain` `403` when the user is not logged in and accesses routes beyond auth. `AuthorizeFilter` sets all routes as requiring authorization, except those defined by `[AllowAnonymous]`.

## Project Structure

    |__ Controllers
    |   |__ ParentController.cs
    |   |
    |   |__ Admin
    |   |
    |   |__ Client
    |   |   |__ BookingController.cs
    |   |   |__ ProfileController.cs
    |   |
    |   |__ JWT
    |       |__ AuthController.cs
    |
    |__ Data
    |   |__ AppDbContext.cs
    |   |
    |   |__ EntityConfig
    |   |   |__ BookingMap.cs
    |   |   |__ HotelMap.cs
    |   |   |__ UserMap.cs
    |   |
    |   |__ Transaction
    |       |__ AutoMapping.cs
    |
    |__ GlobalHanlders
    |   |__ ApiEfCoreHandler.cs
    |   |__ GlobalEndPointHandler.cs
    |
    |__ InverseMigrations
    |
    |__ Models
    |   |__ Client
    |   |   |__ Booking
    |   |   |   |__ BookingModel.cs
    |   |   |   |__ BookingUpdateModel.cs
    |   |   |
    |   |   |__ User
    |   |       |__ UserModel.cs
    |   |
    |   |__ Entities
    |   |   |__ Booking.cs
    |   |   |__ Hotel.cs
    |   |   |__ User.cs
    |   |
    |   |__ JWT
    |       |__ CommonJwtSettings.cs
    |       |__ LoggedInUserData.cs
    |       |__ LoginCredentials.cs
    |       |__ RegisterCredentials.cs
    |
    |__ Properties
    |   |__ launchSettings.json
    |
    |__ Repositories
    |   |__ BaseRepository.cs
    |   |__ BookingRepository.cs
    |   |__ UsersRepository.cs
    |   |
    |   |__ Interfaces
    |       |__ IBaseRepository.cs
    |       |__ IBookingRepository.cs
    |       |__ IUsersRepository.cs
    |
    |__ Services
    |   |__ Admin
    |   |   |__ Interfaces
    |   |
    |   |__ Client
    |   |   |__ BookingService.cs
    |   |   |__ UserService.cs
    |   |   |
    |   |   |__ Interfaces
    |   |       |__ IBookingService.cs
    |   |       |__ IUserService.cs
    |   |
    |   |__ JWT
    |       |__ AuthManagerService.cs
    |       |__ IAuthManagerService.cs
    |
    |__ appsettings.Development.json
    |__ appsettings.json
    |
    |__ GenerateToken.py
    |
    |__ Program.cs
    |
    |__ ZConnector.csproj
    |__ ZConnector.csproj.user
    |__ ZConnector.http
    |__ ZConnector.sln