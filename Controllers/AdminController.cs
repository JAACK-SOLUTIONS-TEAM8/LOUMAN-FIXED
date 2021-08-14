using Louman.Models.DTOs;
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
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpPost("Add")]
        public IActionResult UpsertAdmin(UpsertAdminDto admin)
        {
            var reponse=_adminRepository.Add(admin);
            if(reponse!=null)
                return Ok(new { Admin = reponse ,statusCode=StatusCodes.Status200OK});
            else
                return Ok(new { Admin = reponse, statusCode = StatusCodes.Status400BadRequest });

        }
        [HttpGet("All")]
        public IActionResult GetAllAdmins()
        {
            var reponse = _adminRepository.GetAll();
            if (reponse != null)
                return Ok(new { Admins = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Admins = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("{id}")]
        public IActionResult GetAdminById([FromRoute] int id)
        {
            var reponse = _adminRepository.GetById(id);
            if (reponse != null)
                return Ok(new { Admin = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Admin = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

    }
}
