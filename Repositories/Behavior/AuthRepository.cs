using Louman.AppDbContexts;
using Louman.Models.DTOs.User;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Behavior
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;

        public AuthRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async  Task<bool> isEmailExist(string email)
        {
            var user = await (from u in _dbContext.Users where u.Email == email select u).FirstOrDefaultAsync();
            if (user != null)
            {
                
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await (
                        from u in _dbContext.Users
                        join ut in _dbContext.UserTypes on u.UserTypeId equals ut.UserTypeId
                        where u.isDeleted == false && u.UserName == loginDto.UserName && u.Password == loginDto.Password
                        select new UserDto
                        {
                            UserId = u.UserId,
                            UserName = u.UserName,
                            Password = u.Password,
                            Surname = u.Surname,
                            AddressId = u.AddressId,
                            CellNumber = u.CellNumber,
                            Email = u.Email,
                            IdNumber = u.IdNumber,
                            Initials = u.Name,
                            UserTypeId = u.UserTypeId,
                            UserType = ut.UserTypeDescription
                        }
                        ).FirstOrDefaultAsync();
            if (user != null)
            {
                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = user.UserId,
                    Operation = "LoggedIn"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(user);
            }
            return null;
        }
        public async Task<bool> ResetPassword(UserInfoDto userInfo)
        {
            var user = await (from u in _dbContext.Users where u.UserName == userInfo.UserName && u.Email == userInfo.Email select u).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Password = userInfo.Password;
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();


                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = user.UserId,
                    Operation = "ResetPassword"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);

        }

        public async Task<bool> LogoutAsync(int userId)
        {
            var auditEntity = new AuditEntity
            {
                Date = DateTime.Now,
                UserId = userId,
                Operation = "LoggedOut"
            };

            await _dbContext.Audits.AddAsync(auditEntity);
            await _dbContext.SaveChangesAsync();

            return true;
        }



    }
}
