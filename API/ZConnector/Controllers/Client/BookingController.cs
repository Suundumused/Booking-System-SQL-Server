using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using ZConnector.GlobalHanlders;
using ZConnector.Models.Client;
using ZConnector.Models.Entities;
using ZConnector.Services.Client.Interfaces;


namespace ZConnector.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
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
                if (id is null) return Unauthorized("Session expired. Try Login again.");

                IEnumerable<Booking>? bookings = await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.ListBooKingsByUserByHotel(Convert.ToInt32(id), hotelId),
                    "Booking"
                );

                if (bookings is null)
                {
                    return NotFound("No booking info found.");
                }
                return Ok(bookings);
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch(Exception es)
            {
                return BadRequest("An internal error occurred while retrieving booking info." + es.Message);
            }
        }

        [HttpPost("new")]
        public async Task<IActionResult> Book([FromBody] BookingModel bookModel)
        {
            try 
            {
                string? id = User.FindFirstValue("id");
                if (id is null) return Unauthorized("Session expired. Try Login again.");

                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _bookingService.Book(bookModel),
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
    }
}