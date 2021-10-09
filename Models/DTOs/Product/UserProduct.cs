using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Product
{
    public class UserProduct
    {
        public int UserId { get; set; }
        public ProductDto Product { get; set; }
    }
}
