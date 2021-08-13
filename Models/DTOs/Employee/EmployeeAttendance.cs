using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Employee
{
    public class EmployeeAttendance
    {
        public string initial { get; set; }
        public string surname { get; set; }
        public string TeamName { get; set; }
        public int Attendance { get; set; }
    }
}
