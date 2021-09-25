using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Enquiry
{
    public class GetEnquiryDto
    {
        public int EnquiryId { get; set; }
        public int EnquiryTypeId { get; set; }
        public int ClientUserId { get; set; }
      //  public int AdminUserId { get; set; }
     //   public string AdminName { get; set; }
        public string ClientName{ get; set; }
        public string EnquiryStatus { get; set; }
        public string EnquiryType { get; set; }
        public string EnquiryMessage { get; set; }
    }
}
