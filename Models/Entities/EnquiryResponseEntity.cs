using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("EnquiryResponse")]
    public class EnquiryResponseEntity
    {
        [Key]
        public int EnquiryResponseId {get;set;}
        public int EnquiryId { get; set; }
        public string EnquiryResponseMessage {get;set;}
        public bool isDeleted { get; set; }

    }
}
