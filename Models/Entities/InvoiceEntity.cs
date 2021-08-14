using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Invoice")]
    public class InvoiceEntity
    {
        [Key]
        public int EnquiryTypeId {get;set;}
        public string EnquiryTypeDescription { get; set; }

    }
}
