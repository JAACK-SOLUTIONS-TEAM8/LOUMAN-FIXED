using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("OrderStatus")]
    public class OrderStatusEntity
    {
        [Key]
        public int OrderStatusId {get;set;}
       public string OrderStatusDescription { get; set; }
}
}
