using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Order")]
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        public int ClientUserId { get; set; }
        public int DeliveryTypeId { get; set; }
        public string PaymentType { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? PickupDate { get; set; }
        public DateTime? PickupTime { get; set; }
        public bool? isDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
