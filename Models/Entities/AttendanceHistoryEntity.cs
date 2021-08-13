using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("AttendanceHistory")]
    public class AttendanceHistoryEntity
    {
        [Key]
        public int AttendanceHistoryId { get; set; }
        public int TeamId { get; set; }
        public DateTime Date { get; set; }
    }
}
