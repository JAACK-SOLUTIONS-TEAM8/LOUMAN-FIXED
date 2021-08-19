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
        Task<EnquiryResponseStatusDto> AddEnquiryResponseStatus(EnquiryResponseStatusDto enquiryResponseStatus);
        Task<List<EnquiryResponseStatusDto>> GetAllEnquiryResponseStatus();
        Task<EnquiryResponseStatusDto> GetEnquiryResponseStatusById(int enquiryResponseStatusId);
        Task<bool> DeleteEnquiryResponseStatus(int enquiryResponseStatusId);
        Task<EnquiryResponseDto> AddEnquiryResponse(EnquiryResponseDto enquiryResponse);
        Task<List<EnquiryResponseDto>> GetAllEnquiryResponse();
        Task<EnquiryResponseDto> GetEnquiryResponseById(int enquiryResponseId);
        Task<bool> DeleteEnquiryResponse(int enquiryResponseId);
        Task<EnquiryDto> AddEnquiry(EnquiryDto enquiry);
        Task<List<GetEnquiryDto>> GetAllEnquiries();
        Task<GetEnquiryDto> GetEnquiryById(int enquiryId);
        Task<bool> DeleteEnquiry(int enquiryId);
        Task<List<GetEnquiryDto>> GetAllAdminEnquiryById(int adminUserId);
        Task<List<GetEnquiryDto>> GetAllAdminEnquiryByEnquiryTypeId(int adminUserId, int enquiryTypeId);
        Task<EnquiryWithResponseDto> GetEnquiryWithResponse(int enquiryId);


    }
    
}
