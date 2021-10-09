using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Employee
{
    public class RegisteredEmployeeDto
    {
        public string IdNumber { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}
