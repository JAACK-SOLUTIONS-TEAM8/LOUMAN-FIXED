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
        }
        

        }
}
