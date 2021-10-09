using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("Enquiry")]
    public class EnquiryEntity
    {
        [Key]

      public int EnquiryId {get;set;}
      public int EnquiryTypeId {get;set;}
      public int ClientUserId {get;set;}
     // public int AdminUserId {get;set;}
      public string EnquiryMessage {get;set;}
      public string EnquiryStatus { get; set; }
      public bool isDeleted { get; set; }
    }
}
