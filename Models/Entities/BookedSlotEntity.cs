using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("BookedSlot")]
    public class BookedSlotEntity
    {
     
        [Key]
        public int BookedSlotId { get; set; }
        public int SlotId { get; set; }
        public int ClientUserId { get; set; }
        public int AdminUserId { get; set; }
        public DateTime BookingTime { get; set; }
        public bool isDeleted { get; set; }
    }
}
