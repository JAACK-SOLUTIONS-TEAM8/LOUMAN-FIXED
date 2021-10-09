using Louman.AppDbContexts;
using Louman.Models.DTOs.Employee;
using Louman.Models.DTOs.Team;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Behavior
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _dbContext;

        public TeamRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<TeamDto> AddAsync(TeamDto team)
        {
            if (team.TeamId == 0)
            {
                var newTeam = new TeamEntity
                {
                    TeamName = team.TeamName,
                    TeamDescription = team.TeamDescription,
                    LocationId = team.LocationId,
                    MaxEmployees = team.MaxEmployees,
                    StartTime = DateTime.Parse(team.StartTime),
                    EndTime = DateTime.Parse(team.EndTime),
                    isDeleted = false
                };
                await _dbContext.Teams.AddAsync(newTeam);
                await _dbContext.SaveChangesAsync();


                foreach (var day in team.TeamDays)
                {
                    var teamDaysEntity = new TeamDaysEntity
                    {
                        DayId = day.DayId,
                        TeamId = newTeam.TeamId
                    };
                    await _dbContext.TeamDays.AddAsync(teamDaysEntity);
                    await _dbContext.SaveChangesAsync();

                }

                var location = await _dbContext.Locations.FindAsync(team.LocationId);
                return new TeamDto
                {
                    TeamId = newTeam.TeamId,
                    TeamName = team.TeamName,
                    TeamDescription = team.TeamDescription,
                    LocationId = team.LocationId,
                    MaxEmployees = team.MaxEmployees,
                    StartTime = team.StartTime,
                    EndTime = team.EndTime,
                    locationArea = location.LocationArea,
                    TeamDays = team.TeamDays
                };

            }
            else
            {

                var existingTeam = await (from t in _dbContext.Teams where t.TeamId == team.TeamId && t.isDeleted == false select t).SingleOrDefaultAsync();
                var location = await _dbContext.Locations.FindAsync(team.LocationId);
                if (existingTeam != null)
                {
                    existingTeam.TeamName = team.TeamName;
                    existingTeam.TeamDescription = team.TeamDescription;
                    existingTeam.LocationId = team.LocationId;
                    existingTeam.MaxEmployees = team.MaxEmployees;
                    existingTeam.StartTime = DateTime.Parse(team.StartTime);
                    existingTeam.EndTime = DateTime.Parse(team.EndTime);
                    _dbContext.Update(existingTeam);
                    await _dbContext.SaveChangesAsync();

                    var teamDays = await (from d in _dbContext.TeamDays where d.TeamId == team.TeamId select d).ToListAsync();

                    foreach (var day in teamDays)
                    {
                        _dbContext.Remove(day);
                        await _dbContext.SaveChangesAsync();
                    }
                    foreach (var day in team.TeamDays)
                    {
                        var teamDaysEntity = new TeamDaysEntity
                        {
                            DayId = day.DayId,
                            TeamId = team.TeamId
                        };
                        await _dbContext.TeamDays.AddAsync(teamDaysEntity);
                        await _dbContext.SaveChangesAsync();

                    }
                    return new TeamDto
                    {
                        TeamId = existingTeam.TeamId,
                        TeamName = existingTeam.TeamName,
                        TeamDescription = existingTeam.TeamDescription,
                        LocationId = existingTeam.LocationId,
                        MaxEmployees = existingTeam.MaxEmployees,
                        StartTime = existingTeam.StartTime.TimeOfDay.ToString(),
                        EndTime = existingTeam.EndTime.TimeOfDay.ToString(),
                        locationArea = location.LocationArea,
                        NumberOfEmployees = existingTeam.NumberOfEmployees,
                        TeamDays = team.TeamDays
                    };
                }
            }
            return new TeamDto();
        }

        public async Task<List<TeamDto>> GetAllAsync()
        {

            var teams = await (from t in _dbContext.Teams
                               join l in _dbContext.Locations on t.LocationId equals l.LocationId
                               where t.isDeleted == false
                               orderby t.TeamName
                               select new TeamDto
                               {
                                   TeamId = t.TeamId,
                                   TeamName = t.TeamName,
                                   TeamDescription = t.TeamDescription,
                                   LocationId = t.LocationId,
                                   MaxEmployees = t.MaxEmployees,
                                   StartTime = t.StartTime.ToString("F"),
                                   EndTime = t.EndTime.ToString("F"),
                                   NumberOfEmployees = t.NumberOfEmployees,
                                   locationArea = l.LocationArea
                               }).ToListAsync();


            foreach (var team in teams)
            {

                var teamDays = await (from td in _dbContext.TeamDays
                                      join d in _dbContext.Days on td.DayId equals d.DayId
                                      where td.TeamId == team.TeamId
                                      select new DayDto
                                      {
                                          DayId = td.DayId,
                                          DayName = d.DayName
                                      }).ToListAsync();
                team.TeamDays = teamDays;

            }

            return await Task.FromResult(teams);
        }

        public bool CanMarkAttendance(int teamId)
        {
            var teamWorkingDays= (from d in _dbContext.Days
                                 join td in _dbContext.TeamDays on d.DayId equals td.DayId
                                 where td.TeamId==teamId select new DayDto
                                 {
                                     DayId=d.DayId,
                                     DayName=d.DayName
                                 }).ToList();

            return teamWorkingDays.Any(day => day.DayName.ToLower() == DateTime.Now.DayOfWeek.ToString().ToLower());
        }

        public async Task<List<AttendanceDto>> GetAttendanceData(int teamId)
        {


            var todayRecord = await (from ah in _dbContext.AttendanceHistoryEntities
                                     where ah.TeamId == teamId && ah.Date.Date == DateTime.Now.Date
                                     select ah).SingleOrDefaultAsync();

            if (todayRecord == null)
            {
                var attendanceHistoryEntity = new AttendanceHistoryEntity
                {
                    Date = DateTime.Now,
                    TeamId = teamId
                };
                await _dbContext.AttendanceHistoryEntities.AddAsync(attendanceHistoryEntity);
                await _dbContext.SaveChangesAsync();

                var teamMembers = await (from et in _dbContext.EmployeeTeams
                                         where et.TeamId == teamId
                                         select et).ToListAsync();

                foreach (var member in teamMembers)
                {
                    var attendanceEntity = new AttendanceEntity
                    {
                        AttendanceHistoryId = attendanceHistoryEntity.AttendanceHistoryId,
                        EmployeeId = member.EmployeeId,
                        Absent = false,
                        Present = true,
                        Reason = ""
                    };

                    await _dbContext.AttendanceEntities.AddAsync(attendanceEntity);
                    await _dbContext.SaveChangesAsync();
                }

                var attendanceRecord = await (from a in _dbContext.AttendanceEntities
                                              join ah in _dbContext.AttendanceHistoryEntities on a.AttendanceHistoryId equals ah.AttendanceHistoryId
                                              join e in _dbContext.Employees on a.EmployeeId equals e.EmployeeId
                                              join u in _dbContext.Users on e.UserId equals u.UserId
                                              where u.isDeleted == false && ah.TeamId == teamId
                                              select new AttendanceDto
                                              {
                                                  UserId=u.UserId,
                                                  AttendanceId = a.AttendanceId,
                                                  AttendanceHistoryId = ah.AttendanceHistoryId,
                                                  Absent = a.Absent,
                                                  Present = a.Present,
                                                  Date = ah.Date,
                                                  EmployeeId = a.EmployeeId,
                                                  Initials = u.Name,
                                                  Surname = u.Surname,
                                                  Reason = a.Reason,
                                                  TeamId = teamId
                                              }).ToListAsync();
                return attendanceRecord;

            }
            else
            {

                var attendanceRecord = await (from a in _dbContext.AttendanceEntities
                                              join ah in _dbContext.AttendanceHistoryEntities on a.AttendanceHistoryId equals ah.AttendanceHistoryId
                                              join e in _dbContext.Employees on a.EmployeeId equals e.EmployeeId
                                              join u in _dbContext.Users on e.UserId equals u.UserId
                                              where u.isDeleted == false && ah.Date.Date == DateTime.Now.Date && ah.TeamId == teamId
                                              select new AttendanceDto
                                              {
                                                  AttendanceId = a.AttendanceId,
                                                  AttendanceHistoryId = ah.AttendanceHistoryId,
                                                  Absent = a.Absent,
                                                  Present = a.Present,
                                                  Date = ah.Date,
                                                  EmployeeId = a.EmployeeId,
                                                  Initials = u.Name,
                                                  Surname = u.Surname,
                                                  Reason = a.Reason,
                                                  TeamId = teamId
                                              }).ToListAsync();
                return attendanceRecord;

            }
        }
        public async Task<bool> MarkAttendance(List<AttendanceDto> attendances) 
        {
            foreach (var attendance in attendances)
            {
                var attendanceEntity = await (from a in _dbContext.AttendanceEntities
                                              where a.AttendanceId == attendance.AttendanceId && a.EmployeeId == attendance.EmployeeId
                                              select a).SingleAsync();
                attendanceEntity.Absent = attendance.Absent;
                attendanceEntity.Present = attendance.Present;
                attendanceEntity.Reason = attendance.Reason;

                _dbContext.AttendanceEntities.Update(attendanceEntity);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        public async Task<TeamDto> GetByIdAsync(int teamId)  
        {
            var team = await (from t in _dbContext.Teams
                              join l in _dbContext.Locations on t.LocationId equals l.LocationId
                              where t.isDeleted == false && t.TeamId == teamId
                              orderby t.TeamName
                              select new TeamDto
                              {
                                  TeamId = t.TeamId,
                                  TeamName = t.TeamName,
                                  TeamDescription = t.TeamDescription,
                                  LocationId = t.LocationId,
                                  MaxEmployees = t.MaxEmployees,
                                  StartTime = t.StartTime.ToString("F"),
                                  EndTime = t.EndTime.ToString("F"),
                                  locationArea = l.LocationArea
                              }).SingleOrDefaultAsync();

            var teamDays = await (from td in _dbContext.TeamDays
                                  join d in _dbContext.Days on td.DayId equals d.DayId
                                  where td.TeamId == team.TeamId
                                  select new DayDto
                                  {
                                      DayId = td.DayId,
                                      DayName = d.DayName
                                  }).ToListAsync();
            team.TeamDays = teamDays;

            return await Task.FromResult(team);
        }

        public async Task<List<EmployeeDto>> GetTeamEmployees(int teamId)  
        {
            return await (from u in _dbContext.Users
                          join e in _dbContext.Employees on u.UserId equals e.UserId
                          join et in _dbContext.EmployeeTeams on e.EmployeeId equals et.EmployeeId
                          where et.TeamId == teamId && u.isDeleted == false
                          orderby u.UserName
                          select new EmployeeDto
                          {
                              UserId = e.UserId,
                              EmployeeId = e.EmployeeId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Name,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CommenceDate = e.CommencementDate,
                              TerminationDate = e.TerminationDate,
                              TerminationReason = e.TerminationReason
                          }).ToListAsync();
        }

        public async Task<bool> RemoveAsync(int teamId)  //remove team
        {
            var team = _dbContext.Teams.Find(teamId);
            if (team != null)
            {
                team.isDeleted = true;
                _dbContext.Teams.Update(team);
                await _dbContext.SaveChangesAsync();

                var teamDays = await (from td in _dbContext.TeamDays
                                      join d in _dbContext.Days on td.DayId equals d.DayId
                                      where td.TeamId == team.TeamId
                                      select td).ToListAsync();

                foreach (var day in teamDays)
                {
                    _dbContext.TeamDays.Remove(day);
                    await _dbContext.SaveChangesAsync();
                }

                return true;
            }

            return false;
        }

        public async Task<List<TeamDto>> SearchByName(string name)  //search team
        {
            var teams = await (from t in _dbContext.Teams
                               join l in _dbContext.Locations on t.LocationId equals l.LocationId
                               where t.isDeleted == false && t.TeamName.StartsWith(name)
                               orderby t.TeamName
                               select new TeamDto
                               {
                                   TeamId = t.TeamId,
                                   TeamName = t.TeamName,
                                   TeamDescription = t.TeamDescription,
                                   LocationId = t.LocationId,
                                   MaxEmployees = t.MaxEmployees,
                                   StartTime = t.StartTime.ToString("F"),
                                   EndTime = t.EndTime.ToString("F"),
                                   locationArea = l.LocationArea
                               }).ToListAsync();

            foreach (var team in teams)
            {

                var teamDays = await (from td in _dbContext.TeamDays
                                      join d in _dbContext.Days on td.DayId equals d.DayId
                                      where td.TeamId == team.TeamId
                                      select new DayDto
                                      {
                                          DayId = td.DayId,
                                          DayName = d.DayName
                                      }).ToListAsync();
                team.TeamDays = teamDays;

            }

            return await Task.FromResult(teams);

        }

        public async Task<List<DayDto>> GetWeekDays()
        {
            return await (from d in _dbContext.Days select 
                         new DayDto 
                         { 
                             DayId=d.DayId,
                             DayName=d.DayName
                         }).ToListAsync();

        }
        public async Task<List<AttendanceDto>> GetAttendanceDataForReport(int teamId, string date)
        {
            var todayRecord = await (from ah in _dbContext.AttendanceHistoryEntities
                                     where ah.TeamId == teamId && ah.Date.Date == DateTime.Parse(date).Date
                                     select ah).SingleOrDefaultAsync();

            if (todayRecord == null)
            {

                return new List<AttendanceDto>();

            }
            else
            {

                var attendanceRecord = await (from a in _dbContext.AttendanceEntities
                                              join ah in _dbContext.AttendanceHistoryEntities on a.AttendanceHistoryId equals ah.AttendanceHistoryId
                                              join e in _dbContext.Employees on a.EmployeeId equals e.EmployeeId
                                              join u in _dbContext.Users on e.UserId equals u.UserId
                                              where u.isDeleted == false && ah.Date.Date == DateTime.Parse(date).Date && ah.TeamId == teamId
                                              select new AttendanceDto
                                              {
                                                  AttendanceId = a.AttendanceId,
                                                  AttendanceHistoryId = ah.AttendanceHistoryId,
                                                  Absent = a.Absent,
                                                  Present = a.Present,
                                                  Date = ah.Date,
                                                  EmployeeId = a.EmployeeId,
                                                  Initials = u.Name,
                                                  Surname = u.Surname,
                                                  Reason = a.Reason,
                                                  TeamId = teamId
                                              }).ToListAsync();
                return attendanceRecord;

            }
        }

        public async Task<bool> RemoveEmployeeFromTeam(int teamId, int employeeId)
        {
            var employee = await (from et in _dbContext.EmployeeTeams where (et.TeamId == teamId && et.EmployeeId == employeeId) select et).SingleOrDefaultAsync();

            _dbContext.Remove(employee);
            await _dbContext.SaveChangesAsync();

            var existingTeam = await _dbContext.Teams.FindAsync(employee.TeamId);
            if (existingTeam.NumberOfEmployees > 0)
            {
                existingTeam.NumberOfEmployees -= 1;
                _dbContext.Teams.Update(existingTeam);
                await _dbContext.SaveChangesAsync();
            }
            return true;
        }

        public async Task<List<TeamEmployeeDto>> AddTeamEmployee(TeamEmployeeDto employee)
        {
            var existingTeam = await _dbContext.Teams.FindAsync(employee.TeamId);

            var employeesInTeams = (from e in _dbContext.Employees
                                    join et in _dbContext.EmployeeTeams on e.EmployeeId equals et.EmployeeId
                                    join t in _dbContext.Teams on et.TeamId equals t.TeamId
                                    where t.isDeleted == false
                                    select new { EmployeeId = e.EmployeeId }).ToList();

            if ((existingTeam.NumberOfEmployees < existingTeam.MaxEmployees) && !employeesInTeams.Any(e=>e.EmployeeId==employee.EmployeeId))
            {

                var employeeTeamEntity = new EmployeeTeamEntity
                {
                    EmployeeId = employee.EmployeeId,
                    TeamId = employee.TeamId
                };

                await _dbContext.EmployeeTeams.AddAsync(employeeTeamEntity);
                await _dbContext.SaveChangesAsync();

                existingTeam.NumberOfEmployees += 1;
                _dbContext.Teams.Update(existingTeam);
                await _dbContext.SaveChangesAsync();

                return await (from u in _dbContext.Users
                              join e in _dbContext.Employees on u.UserId equals e.UserId
                              join et in _dbContext.EmployeeTeams on e.EmployeeId equals et.EmployeeId
                              where u.isDeleted == false && et.TeamId == employee.TeamId
                              orderby u.UserName
                              select new TeamEmployeeDto
                              {
                                  UserId = e.UserId,
                                  EmployeeId = e.EmployeeId,
                                  AddressId = u.AddressId,
                                  CellNumber = u.CellNumber,
                                  Email = u.Email,
                                  IdNumber = u.IdNumber,
                                  Initials = u.Name,
                                  Password = u.Password,
                                  Surname = u.Surname,
                                  UserName = u.UserName,
                                  UserTypeId = u.UserTypeId,
                                  CommenceDate = e.CommencementDate,
                                  TerminationDate = e.TerminationDate,
                                  TerminationReason = e.TerminationReason,
                                  TeamId = employee.TeamId

                              }).ToListAsync();

            }

            return null;

            
        }

        public bool CheckTeamValidity(CheckTeamDto team)
        {
            var teams = (from t in _dbContext.Teams where t.isDeleted == false select t).ToList();

            foreach (var t in teams)
            {
                var teamDays = (from td in _dbContext.TeamDays
                               where td.TeamId == t.TeamId
                               select td.DayId).ToList();



                var startTime = DateTime.Parse(team.StartTime);
                var endTime = DateTime.Parse(team.EndTime);

                if (
                     (teamDays.Contains(team.TeamDay))&&
                    (team.LocationId == t.LocationId) &&
                    ((t.StartTime.TimeOfDay < startTime.TimeOfDay) && (t.EndTime.TimeOfDay >= startTime.TimeOfDay) ||
                    (t.StartTime.TimeOfDay >= startTime.TimeOfDay) && (t.EndTime.TimeOfDay <= endTime.TimeOfDay) ||
                    (t.StartTime.TimeOfDay <= endTime.TimeOfDay) && (t.EndTime.TimeOfDay > endTime.TimeOfDay))
                    )
                {
                    return false;
                }
            }
            return true;
        }
    }



}
