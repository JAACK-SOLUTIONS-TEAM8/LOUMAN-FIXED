using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Team
{
    public class TeamDaysDto
    {
        public int DayId { get; set; }
        public int TeamDaysId { get; set; }
        public int TeamId { get; set; }
        public string DayName { get; set; }
    }
}
