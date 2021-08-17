using Louman.Models.DTOs.Admin;
using Louman.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IUserRepository
    {
        Task<UserTypeDto> AddUserType(UserTypeDto userType);
       

    }
}
