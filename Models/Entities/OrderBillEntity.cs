using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("OrderBill")]
    public class OrderBillEntity
    {
        [Key]
        public int BillId { get; set; }
        public int? OrderId { get; set; }
        public decimal? Total { get; set; }
        public decimal? Discount { get; set; }
    }
}
