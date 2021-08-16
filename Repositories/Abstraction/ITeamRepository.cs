using Louman.Models.DTOs.Employee;
using Louman.Models.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface ITeamRepository
    {
        Task<TeamDto> AddAsync(TeamDto team);
        Task<List<TeamDto>> GetAllAsync();
        Task<List<AttendanceDto>> GetAttendanceData(int teamId);
        Task<bool> MarkAttendance(List<AttendanceDto> attendances);
        Task<TeamDto> GetByIdAsync(int teamId);





    }
}
