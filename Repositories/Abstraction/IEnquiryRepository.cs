using Louman.Models.DTOs.Enquiry;
using Louman.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IEnquiryRepository
    {
        Task<EnquiryTypeDto> AddEnquiryType(EnquiryTypeDto enquiryType);
        Task<List<EnquiryTypeDto>> GetAllEnquiryTypes();
        Task<EnquiryTypeDto> GetEnquiryTypeById(int enquiryTypeId);
        Task<bool> DeleteEnquiryType(int enquiryType);


    }
    
}
