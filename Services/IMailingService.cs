using Louman.Models.DTOs.Client;
using Louman.Models.DTOs.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using Louman.Models.DTOs.User;
using System.Threading.Tasks;

namespace Louman.Services
{
    public interface IMailingService
    {
        
        Task SendEmailAsync(ClientDto client);
        Task SendEmailVerificationCode(UserDto user, string code);
    }
}
