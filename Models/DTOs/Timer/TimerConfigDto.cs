using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Timer
{
    public class TimerConfigDto
    {
        public int Id { get; set; }
        public decimal LeftTime { get; set; }
        public bool Demand { get; set; }
        public int Notify { get; set; }
        public decimal StopTime { get; set; }
    }
}
