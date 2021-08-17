using Louman.Models.DTOs.Employee;
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
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        [HttpPost("Upsert")]
        public async Task<IActionResult> Add([FromBody] EmployeeDto employee)
        {
            var emp = await _employeeRepository.Add(employee);
            if (emp != null)
                return Ok(new { Employee = emp, statusCode = StatusCodes.Status200OK });
            return Ok(new { Employee = emp, statusCode = StatusCodes.Status400BadRequest });
        }

        

    }
}
