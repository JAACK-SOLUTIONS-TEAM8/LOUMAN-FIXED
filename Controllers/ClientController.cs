using Louman.Models.DTOs.Client;
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
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _clientRepository.GetAllAsync();
            if (clients != null)
                return Ok(new {clients=clients,statusCode=StatusCodes.Status200OK });
            return Ok(new { clients = clients, statusCode = StatusCodes.Status404NotFound });
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] ClientDto client)
        {
            var clients = await _clientRepository.Add(client);
            if (clients != null)
                return Ok(new { Client = clients, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Client = clients, StatusCode = StatusCodes.Status400BadRequest });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var clients = await _clientRepository.GetByIdAsync(id);
            if (clients != null)
                return Ok(new { Client = clients, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Client = clients, StatusCode = StatusCodes.Status400BadRequest });
        }

        [HttpGet("Clients")]
        public async Task<IActionResult> GetById([FromQuery] string name)
        {
            var clients = await _clientRepository.SearchByNameAsync(name);
            if (clients != null)
                return Ok(new { clients = clients, statusCode = StatusCodes.Status200OK });
            return Ok(new { clients = clients, statusCode = StatusCodes.Status400BadRequest });
        }

        [HttpGet("Users/{id}")]
        public async Task<IActionResult> GetByUserId([FromRoute] int id)
        {
            var clients = await _clientRepository.GetByUserIdAsync(id);
            if (clients != null)
                return Ok(new { Client = clients, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Client = clients, StatusCode = StatusCodes.Status400BadRequest });
        }

    }
}
