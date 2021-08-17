using Louman.AppDbContexts;
using Louman.Models.DTOs.Admin;
using Louman.Models.DTOs.User;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UserTypeDto> AddUserType(UserTypeDto userType)
        {
            if (userType.UserTypeId == 0)
            {
                var newUserType = new UserTypeEntity
                {
                   UserTypeDescription=userType.UserTypeDescription,
                    isDeleted = false
                };
                _dbContext.UserTypes.Add(newUserType);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new UserTypeDto
                {
                    UserTypeId=newUserType.UserTypeId,
                    UserTypeDescription=userType.UserTypeDescription
                });

            }
            else
            {

                var existingUserType = await(from ut in _dbContext.UserTypes where userType.UserTypeId == userType.UserTypeId && ut.isDeleted == false select ut).SingleOrDefaultAsync();
                if (existingUserType != null)
                {
                    existingUserType.UserTypeDescription = userType.UserTypeDescription;
                    _dbContext.Update(existingUserType);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new UserTypeDto
                    {
                        UserTypeId = existingUserType.UserTypeId,
                        UserTypeDescription = userType.UserTypeDescription
                    });
                }
            }
            return new UserTypeDto();

        }

        public async Task<List<UserTypeDto>> GetAllUserTypes()
        {
            return await (from ut in _dbContext.UserTypes
                          where ut.isDeleted == false
                          orderby ut.UserTypeDescription
                          select new UserTypeDto
                          {
                              UserTypeId = ut.UserTypeId,
                              UserTypeDescription = ut.UserTypeDescription
                          }).ToListAsync();
        }

        public async Task<UserTypeDto> GetUserTypeById(int userTypeId)
        {
            return await (from ut in _dbContext.UserTypes
                          where ut.isDeleted == false && ut.UserTypeId == userTypeId
                          orderby ut.UserTypeDescription
                          select new UserTypeDto
                          {
                              UserTypeId = ut.UserTypeId,
                              UserTypeDescription = ut.UserTypeDescription
                          }).SingleOrDefaultAsync();
        }

        public async Task<bool> DeleteUserType(int userTypeId)
        {
            var userType = _dbContext.UserTypes.Find(userTypeId);
            if (userType != null)
            {
                userType.isDeleted = true;
                _dbContext.UserTypes.Update(userType);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<AuditDto>> GetAuditDetail()
        {
            return await (from at in _dbContext.Audits
                          join u in _dbContext.Users on at.UserId equals u.UserId
                          orderby at.Date descending
                          select new AuditDto
                          {
                              AuditId = at.AuditId,
                              Date = at.Date,
                              Operation = at.Operation,
                              UserId = at.UserId,
                              UserName = $"{u.Initials} {u.Surname}",
                          }).ToListAsync();
        }

    }
}
