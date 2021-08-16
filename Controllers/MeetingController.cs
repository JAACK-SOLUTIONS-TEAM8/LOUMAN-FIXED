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
        [HttpGet("Slot/Delete/{id}")]
        public async Task<IActionResult> DeleteSlot([FromRoute] int id)
        {
            var isDeleted = await _meetingRepository.DeleteSlot(id);
            if (isDeleted != false)
                return Ok(new { response = true, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { response = false, StatusCode = StatusCodes.Status404NotFound });
        }


    }
}
