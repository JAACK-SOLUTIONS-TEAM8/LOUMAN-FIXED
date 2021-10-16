using Louman.Models.DTOs;
using Louman.Models.DTOs.Timer;
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
        [HttpGet("Search")]
        public IActionResult SearchByName([FromQuery] string name)
        {
            var reponse = _adminRepository.SearchByName(name);
            if (reponse != null)
                return Ok(new { Admins = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Admins = reponse, statusCode = StatusCodes.Status400BadRequest });

        }
        [HttpGet("Delete/{id}")]
        public IActionResult DeleteAdmin([FromRoute] int id)
        {
            var reponse = _adminRepository.Delete(id);
            if (reponse != false)
                return Ok(new { Admin = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Admin = reponse, statusCode = StatusCodes.Status204NoContent });

        }


        [HttpGet("GetTimerConfig")]
        public IActionResult GetTimerConfig()
        {
            var reponse = _adminRepository.GetTimerCongif();
            if (reponse != null)
                return Ok(new { Config = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Config = reponse, statusCode = StatusCodes.Status400BadRequest });

        }


        [HttpPost("SetTimerConfig")]
        public IActionResult SetTimerConfig(TimerConfigDto config)
        {
            var reponse = _adminRepository.SetTimerConfig(config);
            if (reponse == true)
                return Ok(new { Config = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Config = reponse, statusCode = StatusCodes.Status400BadRequest });

        }


        [HttpPost("Roles/Add")]
        public IActionResult AddRole(RoleDto role)
        {
            var reponse = _adminRepository.AddRole(role);
            if (reponse != null)
                return Ok(new { Role = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Role = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetRoles()
        {
            var reponse = await _adminRepository.GetAllRoles();
            if (reponse != null)
                return Ok(new { Roles = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Roles = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("Roles/{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] int id)
        {
            var reponse = await _adminRepository.GetRoleById(id);
            if (reponse != null)
                return Ok(new { Role = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Role = reponse, statusCode = StatusCodes.Status400BadRequest });

        }
         [HttpGet("Features")]
        public async Task<IActionResult> GetFeatures()
        {
            var reponse = await _adminRepository.GetAllFeatures();
            if (reponse != null)
                return Ok(new { Features = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Features = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

        [HttpGet("Features/{id}")]
        public async Task<IActionResult> GetFeatureById([FromRoute] int id)
        {
            var reponse = await _adminRepository.GetFeatureById(id);
            if (reponse != null)
                return Ok(new { Feature = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Feature = reponse, statusCode = StatusCodes.Status400BadRequest });

        }
        [HttpPost("Features/Add")]
        public async Task<IActionResult> AddFeature(FeatureDto feature)
        {
            var reponse = await _adminRepository.AddFeature(feature);
            if (reponse != null)
                return Ok(new { Feature = reponse, statusCode = StatusCodes.Status200OK });
            else
                return Ok(new { Feature = reponse, statusCode = StatusCodes.Status400BadRequest });

        }

    }
}
