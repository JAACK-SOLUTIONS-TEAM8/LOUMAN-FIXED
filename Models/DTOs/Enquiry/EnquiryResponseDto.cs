using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Models.DTOs.Enquiry
{
    public class EnquiryResponseDto
    {
        public int EnquiryResponseId { get; set; }
        public string EnquiryResponseMessage{ get; set; }
        public int EnquiryId { get; set; }

    }
}
