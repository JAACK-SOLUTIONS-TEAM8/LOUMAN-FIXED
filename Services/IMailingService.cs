using Louman.Models.DTOs.Client;
using Louman.Models.DTOs.Email;
using Louman.Models.DTOs.User;
using Louman.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Services
{
    public interface IMailingService
    {
        
        Task SendEmailAsync(ClientDto client);
        Task SendEmailVerificationCode(UserWithRolesDto user,string code);
    }
}
