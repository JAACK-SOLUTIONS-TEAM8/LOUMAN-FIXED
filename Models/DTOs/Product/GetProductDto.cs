using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public int ProductSizeId { get; set; }
        public string ProductSizeDescription { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
