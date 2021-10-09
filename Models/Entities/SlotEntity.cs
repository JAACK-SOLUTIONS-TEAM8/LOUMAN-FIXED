using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Slot")]
    public class SlotEntity
    {
        [Key]
        public int SlotId { get; set; }
        public int AdminUserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }
        public bool isBooked { get; set; }
        public bool isDeleted { get; set; }
    }
}
