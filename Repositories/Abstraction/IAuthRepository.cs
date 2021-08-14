using Louman.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IAuthRepository
    {
        Task<UserDto> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ResetPassword(UserInfoDto info);
        Task<bool> isEmailExist(string email);
    }
}
