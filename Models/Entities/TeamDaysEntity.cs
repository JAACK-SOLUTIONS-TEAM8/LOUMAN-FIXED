using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("TeamDays")]
    public class TeamDaysEntity
    {
        [Key]
        public int TeamDaysId { get; set; }
        public int DayId { get; set; }
        public int TeamId { get; set; }
    }
}
