using Louman.Models.DTOs.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Team
{
    public class TeamEmployeeDto
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public int? AddressId { get; set; }

        public string IdNumber { get; set; }

        public string Initials { get; set; }
        public string Surname { get; set; }
        public string CellNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int EmployeeId { get; set; }
        public DateTime? CommenceDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationReason { get; set; }
    }
}
