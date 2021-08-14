using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Payment")]
    public class PaymentEntity
    {
        [Key]
      public int  PaymentId { get; set; }
      public int PaymentTypeId { get; set; }
    public int PaymentStatusId { get; set; }
    public int ClientId{ get; set; }
    public int OrderId{ get; set; }
    public decimal PaymentAmount{ get; set; }
    }
}
