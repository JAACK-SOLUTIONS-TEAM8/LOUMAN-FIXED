using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Team")]
    public class TeamEntity
    {
        [Key]
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int LocationId { get; set; }
        public string TeamDescription { get; set; }
        public int MaxEmployees { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool isDeleted { get; set; }


    }
}
