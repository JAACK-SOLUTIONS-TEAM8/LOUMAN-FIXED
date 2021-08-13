using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Team
{
    public class AttendanceDto
    {
        public int AttendanceId { get; set; }
        public int AttendanceHistoryId { get; set; }
        public int TeamId { get; set; }
        public DateTime Date { get; set; }
        public int EmployeeId { get; set; }
        public string Initials { get; set; }
        public string Surname { get; set; }
        public bool Present { get; set; }
        public bool Absent { get; set; }
        public string Reason { get; set; }
    }
}
