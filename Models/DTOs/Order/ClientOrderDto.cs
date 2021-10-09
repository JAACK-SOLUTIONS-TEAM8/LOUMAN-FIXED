using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Order
{
    public class ClientOrderDto
    {
        public int OrderId { get; set; }
        public int ClientUserId { get; set; }
        public string IdNumber { get; set; }
        public string ClientName { get; set; }
        public string Email { get; set; }
        public int DeliveryTypeId { get; set; }
        public int BillId { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public List<ProductInfo> Products { get; set; }
        public string PaymentType { get; set; }
        public CardDetailDto CardDetail { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryType { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string PickupDate { get; set; }
        public string PickupTime { get; set; }

    }
}
