using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Auth
{
    public enum UserLoginResponseType
    {
        EmailNotConfirmed,
        Authenticated,
        UsernameOrPasswordNotMatched,
        TimeOver,
        IncorrectCode
    }
}
