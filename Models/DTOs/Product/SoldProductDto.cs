using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class SoldProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal TotalSoldPrice { get; set; }
        public int QuantitySold { get; set; }
    }
}
