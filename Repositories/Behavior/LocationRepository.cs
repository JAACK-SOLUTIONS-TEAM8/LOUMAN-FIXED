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
        public async Task<LocationDto> AddAsync(LocationDto location)
        {
            if (location.LocationId == 0)
            {
                var newLocation = new LocationEntity
                {
                    LocationArea=location.LocationArea,
                    LocationProvince=location.LocationProvince,
                    isDeleted = false
                };
                _dbContext.Locations.Add(newLocation);
                await _dbContext.SaveChangesAsync();

                
                return await Task.FromResult(new LocationDto
                {
                    LocationId=newLocation.LocationId,
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

        public async Task<bool> DeleteAsync(int locationId)
        {
            var location = _dbContext.Locations.Find(locationId);
            if (location != null)
            {
                location.isDeleted = true;
                _dbContext.Locations.Update(location);
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
