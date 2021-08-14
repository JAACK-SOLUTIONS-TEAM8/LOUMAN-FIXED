using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class StockDto
    {
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
