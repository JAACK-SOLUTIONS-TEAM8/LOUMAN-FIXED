using Louman.AppDbContexts;
using Louman.Models.DTOs.Location;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Behavior
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _dbContext;

        public LocationRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<LocationDto> AddAsync(UserLocation newLocation)
        {

            var location = newLocation.Location;

                if (location.LocationId == 0)
            {
                var locationEntity = new LocationEntity
                {
                    LocationArea=location.LocationArea,
                    LocationProvince=location.LocationProvince,
                    isDeleted = false
                };
                _dbContext.Locations.Add(locationEntity);
                await _dbContext.SaveChangesAsync();

                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = newLocation.UserId,
                    Operation = $"Location:{location.LocationArea} ${location.LocationProvince} is added to the system"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(new LocationDto
                {
                    LocationId= locationEntity.LocationId,
                    LocationArea = location.LocationArea,
                    LocationProvince = location.LocationProvince
                });

            }
            else
            {

                var existingLocation = await (from l in _dbContext.Locations where l.LocationId == location.LocationId && l.isDeleted == false select l).SingleOrDefaultAsync();
                if (existingLocation != null)
                {
                    existingLocation.LocationArea = location.LocationArea;
                    existingLocation.LocationProvince = location.LocationProvince;
                    _dbContext.Update(existingLocation);
                    await _dbContext.SaveChangesAsync();
                    var auditEntity = new AuditEntity
                    {
                        Date = DateTime.Now,
                        UserId = newLocation.UserId,
                        Operation = $"Location:{location.LocationArea} ${location.LocationProvince} is Updated to the system"
                    };

                    await _dbContext.Audits.AddAsync(auditEntity);
                    await _dbContext.SaveChangesAsync();
                    return await Task.FromResult(new LocationDto
                    {
                        LocationId = location.LocationId,
                        LocationArea = location.LocationArea,
                        LocationProvince = location.LocationProvince
                    });
                }
            }
            return new LocationDto();

        }

        public async Task<bool> DeleteAsync(LocationDeletionDto loc)
        {
            var location = _dbContext.Locations.Find(loc.LocationId);

            var locationsForTeam = (from l in _dbContext.Locations
                                    join t in _dbContext.Teams on l.LocationId equals t.LocationId
                                    where t.isDeleted == false && l.isDeleted == false
                                    select new { LocationId = l.LocationId }).ToList();

            if (location != null && !locationsForTeam.Any(l => l.LocationId == location.LocationId))
            {
                location.isDeleted = true;
                _dbContext.Locations.Update(location);
               await _dbContext.SaveChangesAsync();

                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = loc.UserId,
                    Operation = $"Location:{location.LocationArea} ${location.LocationProvince} is deleted from the system"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<List<LocationDto>> GetAllAsync()
        {
            return await (from l in _dbContext.Locations
                    where l.isDeleted == false
                    orderby l.LocationArea
                    select new LocationDto
                    {
                       LocationArea=l.LocationArea,
                       LocationProvince=l.LocationProvince,
                       LocationId=l.LocationId
                    }).ToListAsync();
        }

        public async Task<List<ProvinceDto>> GetAllProvinces()
        {
            return await(from p in _dbContext.Provinces 
                         select new ProvinceDto { 
                         ProvinceId=p.ProvinceId,
                         ProvinceName=p.ProvinceName
                         }).ToListAsync();
        }

        public async Task<LocationDto> GetByIdAsync(int locationId)
        {
            return await(from l in _dbContext.Locations
                         where l.isDeleted == false && l.LocationId==locationId
                         select new LocationDto
                         {
                             LocationArea = l.LocationArea,
                             LocationProvince = l.LocationProvince,
                             LocationId = l.LocationId

                         }).SingleOrDefaultAsync();
        }

        public async Task<List<LocationDto>> SearchByNameAsync(string area)
        {
            return await(from l in _dbContext.Locations
                         where l.isDeleted == false && (string.IsNullOrEmpty(area) || l.LocationArea.StartsWith(area))
                         orderby l.LocationArea
                         select new LocationDto
                         {
                             LocationArea = l.LocationArea,
                             LocationProvince = l.LocationProvince,
                             LocationId = l.LocationId
                         }).ToListAsync();
        }
    }
}
