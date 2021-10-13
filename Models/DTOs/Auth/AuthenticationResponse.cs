using Louman.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Auth
{
    public class AuthenticationResponse
    {
        public UserLoginResponseType ResponseType { get; set; }
        public UserWithRolesDto User { get; set; }
        public string VerificationCode { get; set; }
    }
}
