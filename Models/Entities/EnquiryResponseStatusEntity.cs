using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.Entities
{
    [Table("EnquiryResponseStatus")]
    public class EnquiryResponseStatusEntity
    {
        [Key]
        public int EnquiryReponseStatusId{get;set;}
        public string EnquiryReponseStatusDescription { get;set;}
        public bool isDeleted { get; set; }

    }
}
