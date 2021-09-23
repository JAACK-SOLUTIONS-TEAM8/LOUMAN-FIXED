using Louman.Models.DTOs.Team;
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
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
           _teamRepository = teamRepository;
        }



        [HttpPost("Add")]
        public async Task<IActionResult> AddNewTeam(TeamDto team)
        {
            var newTeam = await _teamRepository.AddAsync(team);
            if (newTeam != null)
                return Ok(new { Team = newTeam, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Team = newTeam, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("WeekDays")]
        public async Task<IActionResult> GetWeekDays()
        {
            var days = await _teamRepository.GetWeekDays();
            if (days != null)
                return Ok(new { Days = days, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Days = days, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpGet("All")]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamRepository.GetAllAsync();
            if (teams != null)
                return Ok(new { Teams = teams, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Teams = teams, StatusCode = StatusCodes.Status404NotFound });

        }


        [HttpGet("AttendanceData/{id}")]
        public async Task<IActionResult> GetAttendanceRecord(int id)
        {
            var attendance = await _teamRepository.GetAttendanceData(id);
            if (attendance != null)
                return Ok(new { Attendance = attendance, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Attendance = attendance, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpPost("MarkAttendance")]
        public async Task<IActionResult> MarkAttendance(List<AttendanceDto> attendance)
        {
            var isMarked = await _teamRepository.MarkAttendance(attendance);
            if (isMarked != false)
                return Ok(new { Attendance = isMarked, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Attendance = isMarked, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamsById([FromRoute] int id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            if (team != null)
                return Ok(new { Team = team, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Team = team, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("Employee/{id}")]
        public async Task<IActionResult> GetTeamEmployee(int id)
        {
            var employees = await _teamRepository.GetTeamEmployees(id);
            if (employees != null)
                return Ok(new { Employees = employees, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Employees = employees, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int id)
        {
            var response = await _teamRepository.RemoveAsync(id);
            if (response != false)
                return Ok(new { Team = true, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Team = false, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchTeam([FromQuery] string name)
        {
            var teams = await _teamRepository.SearchByName(name);
            if (teams != null)
                return Ok(new { Teams = teams, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Teams = teams, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpPost("Employee/Add")]
        public async Task<IActionResult> AddTeamEmployee(TeamEmployeeDto employee)
        {
            var employees = await _teamRepository.AddTeamEmployee(employee);
            if (employees != null)
                return Ok(new { Employees = employees, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Employees = employees, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("Employee/Remove")] 
        public async Task<IActionResult> RemoveEmployeeFromTeam([FromQuery] int teamId, int employeeId)
        {
            var employees = await _teamRepository.RemoveEmployeeFromTeam(teamId, employeeId);
            if (employees != false)
                return Ok(new { Employees = employees, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Employees = employees, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpGet("AttendanceReportData")]
        public async Task<IActionResult> GetAttendanceData([FromQuery] int teamId, string date)
        {
            var attendance = await _teamRepository.GetAttendanceDataForReport(teamId, date);
            if (attendance != null)
                return Ok(new { Attendance = attendance, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Attendance = attendance, StatusCode = StatusCodes.Status404NotFound });

        }
    }
}
