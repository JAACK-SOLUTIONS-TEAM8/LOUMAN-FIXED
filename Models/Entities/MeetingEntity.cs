using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Meeting")]
    public class MeetingEntity
    {
        [Key]
        public int MeetingId{get;set;}
        public int SlotId { get; set; }
        public int MeetingStatusId{get;set;}
        public int ClientId{get;set;}
        public bool isBooked{ get; set; }
    }
}
