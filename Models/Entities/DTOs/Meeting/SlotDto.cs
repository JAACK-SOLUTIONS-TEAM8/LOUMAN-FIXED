using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Meeting
{
    public class SlotDto
    {
        public int SlotId { get; set; }
        public int AdminUserId { get; set; }
        public bool isBooked { get; set; }
        public DateTime Date { get; set; }
        public string StartTime{ get; set; }
        public string EndTime { get; set; }

    }
}
