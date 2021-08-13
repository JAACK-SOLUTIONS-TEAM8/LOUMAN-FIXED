using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Order
{
    public class GetOrderDto
    {
        public int OrderId { get; set; }
        public int ClientUserId { get; set; }
        public string ClientName { get; set; }
        public string OrderStatus{ get; set; }
        public string DeliveryType { get; set; }
        public string PaymentType { get; set; }
        public string PickupDate { get; set; }
        public string PickupTime { get; set; }
        public int BillId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public DateTime? CreatedDate { get; set; }


    }
}
