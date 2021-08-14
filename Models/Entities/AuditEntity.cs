using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Audit")]
    public class AuditEntity
    {
        [Key]
        public int AuditId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }
    }
}
