using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("ProductSize")]
    public class ProductSizeEntity
    {
        [Key]
        public int ProductSizeId { get; set; }
      public string ProductSizeDescription { get; set; }
        public bool isDeleted { get; set; }

    }
}
