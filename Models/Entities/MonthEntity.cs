using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Month")]
    public class MonthEntity
    {
        [Key]
        public int MonthId { get; set; }
        public string MonthName { get; set; }
    }
}
