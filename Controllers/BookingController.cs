using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketReservation.Models;
using TicketReservation.Services;

namespace TicketReservation.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<List<BookingDetails>> Get()
        {
            return await _bookingService.GetBookingsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDetails>> Get(string id)
        {
            var bookingDetails = await _bookingService.GetBookingDetailsByIdAsync(id);
            if (bookingDetails == null)
            {
                return NotFound();
            }
            return Ok(bookingDetails);
        }

        [HttpGet("mybookings/{CusId}")]
        public async Task<List<BookingDetails>> GetByUserID(string CusId)
        {
            return await _bookingService.GetBookingDetailsByUserIdAsync(CusId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookingDetails bookingDetails)
        {
            await _bookingService.CreateAsync(bookingDetails);
            return CreatedAtAction(nameof(Get), new { id = bookingDetails.Id }, bookingDetails);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] BookingDetails bookingDetails)
        {
            await _bookingService.UpdateBookingAsync(id, bookingDetails);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _bookingService.DeleteAsync(id);
            return NoContent();
        }
    }
}