using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("PaymentType")]
    public class PaymentTypeEntity
    {
        [Key]
      public int  PaymentTypeId { get; set; }
      public string PaymentTypeDescription { get; set; }
    }
}
