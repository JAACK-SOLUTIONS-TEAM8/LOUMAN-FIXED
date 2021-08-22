using Louman.Models.DTOs;
using Louman.Models.DTOs.Meeting;
using Louman.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingController(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [HttpGet("Slot/All")]
        public async Task<IActionResult> GetAllSlots()
        {
            var slots = await _meetingRepository.GetAllSlots();
            if (slots != null)
                return Ok(new { Slots=slots,StatusCode=StatusCodes.Status200OK});
            return NotFound(new { Slots=slots,StatusCode=StatusCodes.Status404NotFound});

        }
        [HttpGet("Slot/{id}")]
        public async Task<IActionResult> GetSlotById([FromRoute] int id)
        {
            var slot = await _meetingRepository.GetBySlotId(id);
            if (slot != null)
                return Ok(new { Slot = slot, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Slot = slot, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpGet("Slot/Delete/{id}")]
        public async Task<IActionResult> DeleteSlot([FromRoute] int id)
        {
            var isDeleted = await _meetingRepository.DeleteSlot(id);
            if (isDeleted != false)
                return Ok(new { response = true, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { response = false, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpGet("Slot/Search")]
        public async Task<IActionResult> SearchSlotByDateName([FromQuery] string date)
        {
            var reponse = await _meetingRepository.SearchSlotByDate(date);
            if (reponse != null)
                return Ok(new { Slots = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Slots = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("AdminSlots/Search")]
        public async Task<IActionResult> SearchSlotByDateName([FromQuery] int adminUserId, string date)
        {
            var reponse = await _meetingRepository.SearchAdminSlotsByDate(adminUserId, date);
            if (reponse != null)
                return Ok(new { Slots = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Slots = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("AdminSlots/BookedSlots/{id}")]
        public async Task<IActionResult> GetAllBookedSlotsByAdmin([FromRoute] int id)
        {
            var slots = await _meetingRepository.GetAllBookedSlotByAdminId(id);
            if (slots != null)
                return Ok(new { Slots = slots, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Slots = slots, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpPost("Slot/Add")]
        public async Task<IActionResult> AddSlot(SlotDto slot)
        {
            var newSlot = await _meetingRepository.AddNewSlot(slot);
            if (newSlot != null)
                return Ok(new { Slot = newSlot, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Slot = newSlot, StatusCode = StatusCodes.Status404NotFound });
        }
        [HttpGet("BookedSlots/Cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            var isBooked = await _meetingRepository.CancelBooking(id);
            if (isBooked != false)
                return Ok(new { Response = isBooked, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Response = isBooked, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("BookedSlots/Delete")]
        public async Task<IActionResult> DeleteBookedSlot(int slotIdId, int clientUserId)
        {
            var isDeleted = await _meetingRepository.DeleteBookedSlot(slotIdId, clientUserId);
            if (isDeleted != false)
                return Ok(new { Response = isDeleted, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Response = isDeleted, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("Slot/Book")]
        public async Task<IActionResult> BookNewSlot([FromQuery] int slotId, int clientUserId)
        {
            var bookedSlot = await _meetingRepository.BookSlot(slotId, clientUserId);
            if (bookedSlot != null)
                return Ok(new { Slot = bookedSlot, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Slot = bookedSlot, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpGet("ClientSlots/BookedSlots/{id}")]
        public async Task<IActionResult> GetAllBookedSlotsByClient([FromRoute] int id)
        {
            var slots = await _meetingRepository.GetAllBookedSlotByClientId(id);
            if (slots != null)
                return Ok(new { Slots = slots, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Slots = slots, StatusCode = StatusCodes.Status404NotFound });

        }
    }
}
