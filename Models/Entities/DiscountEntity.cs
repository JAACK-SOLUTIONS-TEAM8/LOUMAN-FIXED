using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Discount")]
    public class DiscountEntity
    {
        [Key]
        public int DiscountId {get;set;}
        public decimal DiscountAmount { get; set; } 
        public decimal DiscountPercentage { get; set; } 
    }
}
