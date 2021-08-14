using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("ProductType")]
    public class ProductTypeEntity
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public bool isDeleted { get; set; }

    }
}
