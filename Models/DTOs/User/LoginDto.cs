﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.User
{
    public class LoginDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}