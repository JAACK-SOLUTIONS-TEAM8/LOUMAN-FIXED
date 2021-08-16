using Louman.AppDbContexts;
using Louman.Models.DTOs.Client;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class ClientRepository : IClientRepository //mistake
    {
        private readonly AppDbContext _dbContext;

        public ClientRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ClientDto> Add(ClientDto client)
        {
            if (client.ClientId == 0)
            {
                var newAddress = new AddressEntity { CityCode = client.CityCode, CityName = client.CityName, StreetName = client.StreetName, StreetNumber = client.StreetNumber };
                _dbContext.Addresses.Add(newAddress);
                await _dbContext.SaveChangesAsync();


                var user = new UserEntity
                {
                    UserName = client.UserName,
                    AddressId = newAddress.AddressId,
                    CellNumber = client.CellNumber,
                    Email = client.Email,
                    IdNumber = client.IdNumber,
                    Initials = client.Initials,
                    Password = client.Password,
                    Surname = client.Surname,
                    UserTypeId = client.UserTypeId,
                    isDeleted = false
                };
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var newClient = new ClientEntity
                {
                    UserId = user.UserId
                };

                await _dbContext.Clients.AddAsync(newClient);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(
                 new ClientDto
                 {
                     ClientId = newClient.ClientId,
                     UserId = user.UserId,
                     AddressId = user.AddressId,
                     CellNumber = user.CellNumber,
                     Email = user.Email,
                     IdNumber = user.IdNumber,
                     Initials = user.Initials,
                     Password = user.Password,
                     Surname = user.Surname,
                     UserName = user.UserName,
                     UserTypeId = user.UserTypeId,
                     CityCode = newAddress.CityCode,
                     CityName = newAddress.CityName,
                     StreetName = newAddress.StreetName,
                     StreetNumber = newAddress.StreetNumber

                 });

            }
            else
            {

                var address = await _dbContext.Addresses.FindAsync(client.AddressId.Value);
                if (address != null)
                {
                    address.CityCode = client.CityCode;
                    address.CityName = client.CityName;
                    address.StreetName = client.StreetName;
                    address.StreetNumber = client.StreetNumber;
                    _dbContext.Addresses.Update(address);
                    await _dbContext.SaveChangesAsync();
                }

                var user = await (from u in _dbContext.Users where u.UserId == client.UserId && u.isDeleted == false select u).SingleOrDefaultAsync();
                if (user != null)
                {
                    user.UserName = client.UserName;
                    user.CellNumber = client.CellNumber;
                    user.Email = client.Email;
                    user.Password = client.Password;
                    user.Surname = client.Surname;
                    user.UserTypeId = client.UserTypeId;
                    user.Initials = client.Initials;
                    user.AddressId = client.AddressId;
                    user.IdNumber = client.IdNumber;
                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();


                    return await Task.FromResult(new ClientDto
                    {
                        ClientId = client.ClientId,
                        UserId = user.UserId,
                        AddressId = user.AddressId,
                        CellNumber = user.CellNumber,
                        Email = user.Email,
                        IdNumber = user.IdNumber,
                        Initials = user.Initials,
                        Password = user.Password,
                        Surname = user.Surname,
                        UserName = user.UserName,
                        UserTypeId = user.UserTypeId,
                        CityCode = address.CityCode,
                        CityName = address.CityName,
                        StreetName = address.StreetName,
                        StreetNumber = address.StreetNumber
                    });
                }
            }
            return null;

        }
        public async Task<List<ClientDto>> GetAllAsync()
        {
            return await (from u in _dbContext.Users
                          join c in _dbContext.Clients on u.UserId equals c.UserId
                          join a in _dbContext.Addresses on u.AddressId equals a.AddressId
                          where u.isDeleted == false
                          orderby u.UserName
                          select new ClientDto
                          {
                              UserId = c.UserId,
                              ClientId = c.ClientId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CityCode = a.CityCode,
                              CityName = a.CityName,
                              StreetName = a.StreetName,
                              StreetNumber = a.StreetNumber
                          }).ToListAsync();
        }

        public async Task<ClientDto> GetByIdAsync(int clientId)
        {
            return await (from u in _dbContext.Users
                          join c in _dbContext.Clients on u.UserId equals c.UserId
                          join a in _dbContext.Addresses on u.AddressId equals a.AddressId
                          where u.isDeleted == false && c.ClientId == clientId
                          orderby u.UserName
                          select new ClientDto
                          {
                              UserId = c.UserId,
                              ClientId = c.ClientId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CityCode = a.CityCode,
                              CityName = a.CityName,
                              StreetName = a.StreetName,
                              StreetNumber = a.StreetNumber
                          }).SingleOrDefaultAsync();
        }

        public async Task<ClientDto> GetByUserIdAsync(int clientUserId)
        {
            return await (from u in _dbContext.Users
                          join c in _dbContext.Clients on u.UserId equals c.UserId
                          join a in _dbContext.Addresses on u.AddressId equals a.AddressId
                          where u.isDeleted == false && c.UserId == clientUserId
                          orderby u.UserName
                          select new ClientDto
                          {
                              UserId = c.UserId,
                              ClientId = c.ClientId,
                              AddressId = u.AddressId,
                              CellNumber = u.CellNumber,
                              Email = u.Email,
                              IdNumber = u.IdNumber,
                              Initials = u.Initials,
                              Password = u.Password,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              UserTypeId = u.UserTypeId,
                              CityCode = a.CityCode,
                              CityName = a.CityName,
                              StreetName = a.StreetName,
                              StreetNumber = a.StreetNumber
                          }).SingleOrDefaultAsync();
        }
    }
}
