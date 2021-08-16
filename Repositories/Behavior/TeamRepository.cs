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
    }
        

        
}
