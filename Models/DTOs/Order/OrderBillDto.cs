using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Order
{
    public class OrderBillDto
    {
        public int BillId { get; set; }
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Vat { get; set; }

    }
}
