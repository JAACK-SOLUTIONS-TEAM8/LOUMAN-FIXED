using Louman.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Order
{
    public class ProductInfo
    {
        public GetStockProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
