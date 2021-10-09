using Louman.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Auth
{
    public class CodeDto
    {
        public UserDto User { get; set; }
        public string Code { get; set; }
    }
}
