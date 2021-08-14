using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Team
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int LocationId { get; set; }
        public string TeamDescription { get; set; }
        public int MaxEmployees { get; set; }
        public int NumberOfEmployees { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string locationArea { get; set; }
        public List<DayDto> TeamDays { get; set; }

    }
}
