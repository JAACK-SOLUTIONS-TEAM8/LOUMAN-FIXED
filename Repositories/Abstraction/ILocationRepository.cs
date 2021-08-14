using Louman.Models.DTOs.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface ILocationRepository
    {
        Task<LocationDto> AddAsync(LocationDto location);
        Task<List<LocationDto>> GetAllAsync();
        Task<bool> DeleteAsync(int locationId);

        Task<LocationDto> GetByIdAsync(int locationId);

        Task<List<LocationDto>> SearchByNameAsync(string area);
    }
}
