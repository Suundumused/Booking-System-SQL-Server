using Microsoft.AspNetCore.Mvc;

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

        private async Task<IActionResult?> CheckBookingAndAuthority(int bookingId, int userId) 
        {
            int? currentBookingOwnerId = await _bookingService.GetBookingOwnerById(bookingId);
            if (currentBookingOwnerId is null)
            {
                return NotFound("Booking not found.");
            }
            if (currentBookingOwnerId != userId)
            {
                return Unauthorized("Booking does not belong to the user.");
            }

            return null;
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

                if (await CheckBookingAndAuthority((int)bookingModel.ID!, (int)bookingModel.UserID) is not null and IActionResult result) 
                {
                    return result;
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

        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> UnBook(int bookingId) 
        {
            try 
            {
                string? id = User.FindFirstValue("id");
                if (id is null) return AuthExpired();

                if (await CheckBookingAndAuthority(bookingId, Convert.ToInt32(id)) is not null and IActionResult result)
                {
                    return result;
                }

                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.UnBook(bookingId),
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
                return BadRequest("An internal error occurred while unbooking.");
            }
        }
    }
}