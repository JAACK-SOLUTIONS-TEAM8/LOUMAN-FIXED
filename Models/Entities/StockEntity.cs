using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Stock")]
    public class StockEntity
    {
        [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public bool isDeleted { get; set; }
        public DateTime Date { get; set; }

    }
}
