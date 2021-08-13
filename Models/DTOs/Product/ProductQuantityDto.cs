using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class ProductQuantityDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSize{ get; set; }
        public string ProductType { get; set; }
        public int SoldQuantity { get; set; }



    }
}
