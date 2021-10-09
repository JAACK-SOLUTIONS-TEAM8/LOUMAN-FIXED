using Louman.AppDbContexts;
using Louman.Models.DTOs.Auth;
using Louman.Models.DTOs.User;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Louman.Services;
using Louman.Utilities;
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
        private IMailingService _mailingService;
        public AuthRepository(AppDbContext dbContext, IMailingService mailingService)
        {
            _dbContext = dbContext;
            _mailingService = mailingService;
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

        public async Task<AuthenticationResponse> LoginAsync(LoginDto loginDto)
        {

            var user = await (
                        from u in _dbContext.Users
                        join ut in _dbContext.UserTypes on u.UserTypeId equals ut.UserTypeId
                        where u.isDeleted == false && u.UserName == loginDto.UserName && u.Password == Hashing.GenerateSha512String(loginDto.Password)
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
                var usr = (from u in _dbContext.Users where u.UserName == loginDto.UserName &&  u.Password == Hashing.GenerateSha512String(loginDto.Password) && u.isDeleted == false select u).SingleOrDefault();

                Random r = new Random();
                var code = r.Next(1000, 999999).ToString();

                usr.EmailConfirmationCode = code;
                usr.TokenExpirationTime = DateTime.Now.AddMinutes(5);

                _dbContext.Users.Update(usr);
                await _dbContext.SaveChangesAsync();

                await _mailingService.SendEmailVerificationCode(user, code);

                return await Task.FromResult(new AuthenticationResponse { ResponseType=UserLoginResponseType.Authenticated,User= user } );
            }
            return await Task.FromResult(new AuthenticationResponse { ResponseType = UserLoginResponseType.UsernameOrPasswordNotMatched, User = null });
        }
        public async Task<bool> ResetPassword(UserInfoDto userInfo)
        {
            var user = await (from u in _dbContext.Users where u.UserName == userInfo.UserName && u.Email == userInfo.Email select u).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Password = Hashing.GenerateSha512String(userInfo.Password);
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

        public async  Task<AuthenticationResponse> VerifyCode(CodeDto code)
        {
           var user = await(from u in _dbContext.Users where  u.Email == code.User.Email && u.UserName==code.User.UserName select u).FirstOrDefaultAsync();

            if(user!=null)
            {
                if(user.TokenExpirationTime.Value.Subtract(DateTime.Now).Minutes <=5 )
                {
                    
                    if(user.EmailConfirmationCode == code.Code)
                    {
                        var usr = await (
                        from u in _dbContext.Users
                        join ut in _dbContext.UserTypes on u.UserTypeId equals ut.UserTypeId
                        where u.isDeleted == false && u.UserName == code.User.UserName && u.Email == code.User.Email
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
                        }).FirstOrDefaultAsync();

                        if(usr!=null)
                        {
                            return await Task.FromResult(new AuthenticationResponse
                            {
                                ResponseType = UserLoginResponseType.Authenticated,
                                VerificationCode = "",
                                User = usr
                            });
                        }
                    }
                    return await Task.FromResult(new AuthenticationResponse
                    {
                        ResponseType = UserLoginResponseType.IncorrectCode,
                        User = new UserDto
                        {
                            UserId = user.UserId,
                            UserName = user.UserName,
                            Password = user.Password,
                            Surname = user.Surname,
                            AddressId = user.AddressId,
                            CellNumber = user.CellNumber,
                            Email = user.Email,
                            IdNumber = user.IdNumber,
                            Initials = user.Name,
                            UserTypeId = user.UserTypeId,
                            UserType = ""
                        },
                        VerificationCode = ""
                    });
                }
            }
            return await Task.FromResult( new AuthenticationResponse { 
                ResponseType=UserLoginResponseType.TimeOver,
                User=new UserDto {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Password = user.Password,
                    Surname = user.Surname,
                    AddressId = user.AddressId,
                    CellNumber = user.CellNumber,
                    Email = user.Email,
                    IdNumber = user.IdNumber,
                    Initials = user.Name,
                    UserTypeId = user.UserTypeId,
                    UserType = ""
                },
                VerificationCode=""});

        }
    }
}
