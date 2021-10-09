using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs
{
    public class BookedSlotDto
    {
        public int BookedSlotId { get; set; }
        public int SlotId { get; set; }
        public int ClientUserId { get; set; }
        public int AdminUserId { get; set; }
        public string AdminName { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone{ get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
