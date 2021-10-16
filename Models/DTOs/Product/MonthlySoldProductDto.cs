using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class MonthlySoldProductDto
    {
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public List<SoldProductDto> SoldProducts { get; set; }
    }
}
