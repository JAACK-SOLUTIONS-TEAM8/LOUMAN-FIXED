using Louman.Models.DTOs.Enquiry;
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
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryRepository _enquiryRepository;

        public EnquiryController(IEnquiryRepository enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        [HttpPost("EnquiryType/Add")]
        public async Task<IActionResult> AddEnquiryType([FromBody] EnquiryTypeDto enquiryType)
        {
            var enquiry = await _enquiryRepository.AddEnquiryType(enquiryType);
            if (enquiry != null)
                return Ok(new { EnquiryType = enquiry, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryType = enquiry, StatusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("EnquiryType/All")]
        public async Task<IActionResult> GetAllEnquiryTypes()
        {
            var enquiryTypes = await _enquiryRepository.GetAllEnquiryTypes();
            if (enquiryTypes != null)
                return Ok(new { EnquiryTypes = enquiryTypes, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryTypes = enquiryTypes, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("EnquiryType/{id}")]
        public async Task<IActionResult> GetEnquiryTypeById([FromRoute] int id)
        {
            var enquiryType = await _enquiryRepository.GetEnquiryTypeById(id);
            if (enquiryType != null)
                return Ok(new { EnquiryType = enquiryType, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryType = enquiryType, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("EnquiryType/Delete/{id}")]
        public async Task<IActionResult> DelteEnquiryType([FromRoute] int id)
        {
            var response = await _enquiryRepository.DeleteEnquiryType(id);
            if (response != false)
                return Ok(new { EnquiryTypes = response, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryTypes = response, StatusCode = StatusCodes.Status400BadRequest });

        }


        [HttpGet("EnquiryResponseStatus/All")]
        public async Task<IActionResult> GetAllEnquiryResponseStatus()
        {
            var enquiryResponsesStatus = await _enquiryRepository.GetAllEnquiryResponseStatus();
            if (enquiryResponsesStatus != null)
                return Ok(new { EnquiryResponsesStatus = enquiryResponsesStatus, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryResponsesStatus = enquiryResponsesStatus, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("EnquiryResponseStatus/{id}")]
        public async Task<IActionResult> GetEnquiryResponseStatusById([FromRoute] int id)
        {
            var enquiryResponseStatus = await _enquiryRepository.GetEnquiryResponseById(id);
            if (enquiryResponseStatus != null)
                return Ok(new { EnquiryResponseStatus = enquiryResponseStatus, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryResponseStatus = enquiryResponseStatus, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("EnquiryResponseStatus/Delete/{id}")]
        public async Task<IActionResult> DelteEnquiryResponseStatus([FromRoute] int id)
        {
            var response = await _enquiryRepository.DeleteEnquiryResponseStatus(id);
            if (response != true)
                return Ok(new { EnquiryTypes = response, StatusCode = StatusCodes.Status200OK });
            return Ok(new { EnquiryTypes = response, StatusCode = StatusCodes.Status400BadRequest });

        }
    }
}
