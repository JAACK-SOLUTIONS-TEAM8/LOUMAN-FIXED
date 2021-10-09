using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Team
{
    public class CheckTeamDto
    {
        public int TeamId { get; set; }
        public int LocationId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public  int TeamDay { get; set; }
    }
}
