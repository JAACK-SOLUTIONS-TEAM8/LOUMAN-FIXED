using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Employee
{
    public class RegistrationDetail
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public string Year { get; set; }

        public List<RegisteredEmployeeDto> Employees { get; set; }
    }
}
