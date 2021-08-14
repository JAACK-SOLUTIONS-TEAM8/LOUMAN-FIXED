using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Product")]
    public class ProductEntity
    {
        [Key]
        public int ProductId {get;set;}
        public int ProductTypeId { get; set; }
        public int ProductSizeId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public bool isDeleted { get; set; }

    }
}
