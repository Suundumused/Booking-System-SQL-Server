using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

using ZConnector.GlobalHanlders;
using ZConnector.Models.Client.Booking;
using ZConnector.Models.Entities;
using ZConnector.Services.Client.Interfaces;


namespace ZConnector.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ParentController
    {
        private readonly IBookingService _bookingService;


        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetMyBookings(int hotelId)
        {
            try
            {
                string? id = User.FindFirstValue("id");
                if (id is null) return AuthExpired();

                IEnumerable<Booking>? bookings = await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.ListBooKingsByUserByHotel(Convert.ToInt32(id), hotelId),
                    "Booking"
                );

                if (bookings is null)
                {
                    return NotFound("No booking info found.");
                }
                return StatusCode(bookings.Count() == 0 ? 204 : 200, bookings);
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch
            {
                return BadRequest("An internal error occurred while retrieving booking info.");
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> Book([FromBody] BookingModel bookingModel)
        {
            try 
            {
                string? id = User.FindFirstValue("id");
                if (id is null) return AuthExpired();

                bookingModel.UserID = Convert.ToInt32(id);

                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.Book(bookingModel),
                    "Booking"
                );

                return Ok();
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch 
            {
                return BadRequest("An internal error occurred while booking.");
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateBooking([FromBody] BookingUpdateModel bookingModel) 
        {
            try
            {
                string? id = User.FindFirstValue("id");
                if (id is null) return AuthExpired();

                bookingModel.UserID = Convert.ToInt32(id);

                int? currentBookingOwnerId = await _bookingService.GetBookingOwnerById((int)bookingModel.ID!);
                if (currentBookingOwnerId is null) 
                {
                    return NotFound("Booking not found.");
                }
                if (currentBookingOwnerId != bookingModel.UserID) 
                {
                    return Unauthorized("Booking does not belong to the user.");
                }

                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.UpdateBookingInfo(bookingModel),
                    "Booking"
                );

                return Ok();
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch
            {
                return BadRequest("An internal error occurred while updating the booking.");
            }
        }
    }
}