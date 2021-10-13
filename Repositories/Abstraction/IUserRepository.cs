using Louman.Models.DTOs;
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
        Task<List<UserTypeDto>> GetAllUserTypes();
        Task<UserTypeDto> GetUserTypeById(int userTypeId);
        Task<bool> DeleteUserType(int userTypeId);
        Task<List<AuditDto>> GetAuditDetail();
        Task<List<AuditDto>> SearchAuditByUserName(string name);
        Task<List<UserRoleDto>> GetUserRole(int userId);
        Task<bool> AddUserRole(AddRoleDto roleData);
    }
}
