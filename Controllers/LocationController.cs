using Louman.Models.DTOs.Location;
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
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAllLocations()
        {
            var locations = await _locationRepository.GetAllAsync();
            if (locations != null)
                return Ok(new { Locations=locations,StatusCode=StatusCodes.Status200OK});
            return NotFound(new { Locations = locations, StatusCode = StatusCodes.Status200OK });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddLocation(UserLocation newLocation)
        {
            var location = await _locationRepository.AddAsync(newLocation);
            if (location != null)
                return Ok(new { Location = location, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Location = location, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location != null)
                return Ok(new { Location = location, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Location = location, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(LocationDeletionDto location)
        {
            var response = await _locationRepository.DeleteAsync(location);
            if (response == true)
                return Ok(new { Response = true, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Response = false, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchByName([FromQuery] string location)
        {
            var response = await _locationRepository.SearchByNameAsync(location);
            if (response != null)
                return Ok(new { Locations = response, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Locations = response, StatusCode = StatusCodes.Status404NotFound });
        }


        [HttpGet("Provinces")]
        public async Task<IActionResult> GetAllProvinces()
        {
            var response = await _locationRepository.GetAllProvinces();
            if (response != null)
                return Ok(new { Provinces = response, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Provinces = response, StatusCode = StatusCodes.Status404NotFound });
        }



    }
}
