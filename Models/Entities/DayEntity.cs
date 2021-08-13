using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Day")]
    public class DayEntity
    {
        [Key]
        public int DayId {get;set;}
        public string DayName {get;set;}
    }
}
