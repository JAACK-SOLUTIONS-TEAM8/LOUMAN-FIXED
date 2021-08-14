using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("PaymentStatus")]
    public class PaymentStatusEntity
    {
        [Key]
        public int PaymentStatusId {get;set;}
        public string PaymentStatusDescription {get;set;}
    }
}
