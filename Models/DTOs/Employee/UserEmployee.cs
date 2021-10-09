using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Employee
{
    public class UserEmployee
    {
        public int UserId { get; set; }
        public EmployeeDto Employee { get; set; }
    }
}
