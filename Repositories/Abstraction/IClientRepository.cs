using Louman.Models.DTOs.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IClientRepository
    {
        Task<ClientDto> Add(ClientDto client);
        Task<List<ClientDto>> GetAllAsync();
        Task<ClientDto> GetByIdAsync(int clientId);
        Task<ClientDto> GetByUserIdAsync(int clientUserId);
        Task<List<ClientDto>> SearchByNameAsync(string name);
        Task<bool> DeleteAsync(int clientId);


    }
}
