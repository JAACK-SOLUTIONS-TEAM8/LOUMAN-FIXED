using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Employee
{
    public class EmployeeReport
    {
        public string Initials { get; set; }
        public string Surname { get; set; }
        public string TeamName { get; set; }
        public string DaysAttended { get; set; }

    }
}
