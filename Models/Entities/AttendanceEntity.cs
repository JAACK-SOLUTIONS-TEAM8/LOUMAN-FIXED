using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Attendance")]
    public class AttendanceEntity
    {
        [Key]
        public int AttendanceId { get; set; }
        public int EmployeeId{ get; set; }
        public bool Present{ get; set; }
        public bool Absent{ get; set; }
        public int AttendanceHistoryId{ get; set; }
        public string Reason{ get; set; }
    }
}
