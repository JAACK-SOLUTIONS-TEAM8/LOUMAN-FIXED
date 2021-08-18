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

    }
}
