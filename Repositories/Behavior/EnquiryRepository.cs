using Louman.AppDbContexts;
using Louman.Models.DTOs.Enquiry;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Behavior
{
    public class EnquiryRepository : IEnquiryRepository
    {
        private readonly AppDbContext _dbContext;

        public EnquiryRepository(AppDbContext dbContext)
        {
           _dbContext = dbContext;
        }
        public async Task<EnquiryTypeDto> AddEnquiryType(EnquiryTypeDto enquiryType)
        {
            if (enquiryType.EnquiryTypeId == 0)
            {
                var newEnquiryType = new EnquiryTypeEntity
                {
                   EnquiryTypeDescription= enquiryType.EnquiryTypeDescription,
                    isDeleted = false
                };
                _dbContext.EnquiryTypes.Add(newEnquiryType);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new EnquiryTypeDto
                {
                    EnquiryTypeId=newEnquiryType.EnquiryTypeId,
                    EnquiryTypeDescription= enquiryType.EnquiryTypeDescription
                });

            }
            else
            {

                var existingEnquiryType = await(from e in _dbContext.EnquiryTypes where e.EnquiryTypeId == enquiryType.EnquiryTypeId && e.isDeleted == false select e).SingleOrDefaultAsync();
                if (existingEnquiryType != null)
                {
                    existingEnquiryType.EnquiryTypeDescription = enquiryType.EnquiryTypeDescription;
                    _dbContext.Update(existingEnquiryType);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new EnquiryTypeDto
                    {
                        EnquiryTypeDescription= existingEnquiryType.EnquiryTypeDescription,
                        EnquiryTypeId= existingEnquiryType.EnquiryTypeId
                    });
                }
            }
            return new EnquiryTypeDto();

        }

        public async Task<List<EnquiryTypeDto>> GetAllEnquiryTypes()
        {
            return await (from et in _dbContext.EnquiryTypes
                          where et.isDeleted == false
                          orderby et.EnquiryTypeDescription
                          select new EnquiryTypeDto
                          {
                              EnquiryTypeDescription = et.EnquiryTypeDescription,
                              EnquiryTypeId = et.EnquiryTypeId
                          }).ToListAsync();

        }

        public async Task<EnquiryTypeDto> GetEnquiryTypeById(int enquiryTypeId)
        {
            return await (from et in _dbContext.EnquiryTypes
                          where et.isDeleted == false && et.EnquiryTypeId == enquiryTypeId
                          orderby et.EnquiryTypeDescription
                          select new EnquiryTypeDto
                          {
                              EnquiryTypeDescription = et.EnquiryTypeDescription,
                              EnquiryTypeId = et.EnquiryTypeId
                          }).SingleOrDefaultAsync();

        }

        public async Task<bool> DeleteEnquiryType(int enquiryTypeId)
        {
            var enquiryType = _dbContext.EnquiryTypes.Find(enquiryTypeId);
            if (enquiryType != null)
            {
                enquiryType.isDeleted = true;
                _dbContext.EnquiryTypes.Update(enquiryType);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<EnquiryResponseStatusDto> AddEnquiryResponseStatus(EnquiryResponseStatusDto enquiryResponseStatus)
        {
            if (enquiryResponseStatus.EnquiryResponseStatusId == 0)
            {
                var newEnquiryResponseStatus = new EnquiryResponseStatusEntity
                {
                    EnquiryReponseStatusDescription = enquiryResponseStatus.EnquiryResponseStatusDescription,
                    isDeleted = false
                };
                _dbContext.EnquiryResponseStatus.Add(newEnquiryResponseStatus);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new EnquiryResponseStatusDto
                {
                    EnquiryResponseStatusId = newEnquiryResponseStatus.EnquiryReponseStatusId,
                    EnquiryResponseStatusDescription = newEnquiryResponseStatus.EnquiryReponseStatusDescription
                });

            }
            else
            {

                var existingEnquiryResponseStatus = await (from ers in _dbContext.EnquiryResponseStatus where ers.EnquiryReponseStatusId == enquiryResponseStatus.EnquiryResponseStatusId && ers.isDeleted == false select ers).SingleOrDefaultAsync();
                if (existingEnquiryResponseStatus != null)
                {
                    existingEnquiryResponseStatus.EnquiryReponseStatusDescription = enquiryResponseStatus.EnquiryResponseStatusDescription;
                    _dbContext.Update(existingEnquiryResponseStatus);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new EnquiryResponseStatusDto
                    {
                        EnquiryResponseStatusDescription = existingEnquiryResponseStatus.EnquiryReponseStatusDescription,
                        EnquiryResponseStatusId = existingEnquiryResponseStatus.EnquiryReponseStatusId
                    });
                }
            }
            return new EnquiryResponseStatusDto();

        }

        public async Task<List<EnquiryResponseStatusDto>> GetAllEnquiryResponseStatus()
        {
            return await (from ers in _dbContext.EnquiryResponseStatus
                          where ers.isDeleted == false
                          orderby ers.EnquiryReponseStatusDescription
                          select new EnquiryResponseStatusDto
                          {
                              EnquiryResponseStatusDescription = ers.EnquiryReponseStatusDescription,
                              EnquiryResponseStatusId = ers.EnquiryReponseStatusId
                          }).ToListAsync();
        }

        public async Task<EnquiryResponseStatusDto> GetEnquiryResponseStatusById(int enquiryResponseStatusId)
        {
            return await (from ers in _dbContext.EnquiryResponseStatus
                          where ers.isDeleted == false && ers.EnquiryReponseStatusId == enquiryResponseStatusId
                          orderby ers.EnquiryReponseStatusDescription
                          select new EnquiryResponseStatusDto
                          {
                              EnquiryResponseStatusDescription = ers.EnquiryReponseStatusDescription,
                              EnquiryResponseStatusId = ers.EnquiryReponseStatusId
                          }).SingleOrDefaultAsync();
        }

        public async Task<bool> DeleteEnquiryResponseStatus(int enquiryResponseStatusId)
        {
            var enquiryResponsesStatus = _dbContext.EnquiryResponseStatus.Find(enquiryResponseStatusId);
            if (enquiryResponsesStatus != null)
            {
                enquiryResponsesStatus.isDeleted = true;
                _dbContext.EnquiryResponseStatus.Update(enquiryResponsesStatus);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<EnquiryResponseDto> AddEnquiryResponse(EnquiryResponseDto enquiryResponse)
        {
            if (enquiryResponse.EnquiryResponseId == 0)
            {
                var newEnquiryResponse = new EnquiryResponseEntity
                {
                    EnquiryResponseMessage = enquiryResponse.EnquiryResponseMessage,
                    EnquiryId = enquiryResponse.EnquiryId,
                    isDeleted = false
                };
                _dbContext.EnquiryResponses.Add(newEnquiryResponse);
                await _dbContext.SaveChangesAsync();

                var enquiryEntity = await _dbContext.Enquiries.FindAsync(enquiryResponse.EnquiryId);
                enquiryEntity.EnquiryStatus = "Responded";
                _dbContext.Enquiries.Update(enquiryEntity);
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(new EnquiryResponseDto
                {
                    EnquiryResponseId = newEnquiryResponse.EnquiryResponseId,
                    EnquiryResponseMessage = enquiryResponse.EnquiryResponseMessage,
                    EnquiryId = enquiryResponse.EnquiryId
                });

            }
            else
            {

                var existingEnquiryResponse = await (from er in _dbContext.EnquiryResponses where er.EnquiryResponseId == enquiryResponse.EnquiryResponseId && er.isDeleted == false select er).SingleOrDefaultAsync();
                if (existingEnquiryResponse != null)
                {
                    existingEnquiryResponse.EnquiryResponseMessage = enquiryResponse.EnquiryResponseMessage;
                    _dbContext.Update(existingEnquiryResponse);
                    await _dbContext.SaveChangesAsync();

                    var enquiryEntity = await _dbContext.Enquiries.FindAsync(enquiryResponse.EnquiryId);
                    enquiryEntity.EnquiryStatus = "Responded";
                    _dbContext.Enquiries.Update(enquiryEntity);
                    await _dbContext.SaveChangesAsync();


                    return await Task.FromResult(new EnquiryResponseDto
                    {
                        EnquiryResponseId = existingEnquiryResponse.EnquiryResponseId,
                        EnquiryResponseMessage = enquiryResponse.EnquiryResponseMessage,
                        EnquiryId = enquiryResponse.EnquiryId
                    });
                }
            }
            return new EnquiryResponseDto();
        }

        public async Task<List<EnquiryResponseDto>> GetAllEnquiryResponse()
        {
            return await (from er in _dbContext.EnquiryResponses
                          where er.isDeleted == false
                          orderby er.EnquiryResponseMessage
                          select new EnquiryResponseDto
                          {
                              EnquiryResponseMessage = er.EnquiryResponseMessage,
                              EnquiryResponseId = er.EnquiryResponseId
                          }).ToListAsync();
        }

        public async Task<EnquiryResponseDto> GetEnquiryResponseById(int enquiryResponseId)
        {
            return await (from er in _dbContext.EnquiryResponses
                          where er.isDeleted == false && er.EnquiryResponseId == enquiryResponseId
                          orderby er.EnquiryResponseMessage
                          select new EnquiryResponseDto
                          {
                              EnquiryResponseMessage = er.EnquiryResponseMessage,
                              EnquiryResponseId = er.EnquiryResponseId
                          }).SingleOrDefaultAsync();

        }

        public async Task<bool> DeleteEnquiryResponse(int enquiryResponseId)
        {
            var enquiryResponses = _dbContext.EnquiryResponses.Find(enquiryResponseId);
            if (enquiryResponses != null)
            {
                enquiryResponses.isDeleted = true;
                _dbContext.EnquiryResponses.Update(enquiryResponses);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        //
        public async Task<EnquiryDto> AddEnquiry(EnquiryDto enquiry)
        {
            if (enquiry.EnquiryId == 0)
            {
                var newEnquiry = new EnquiryEntity
                {
                    EnquiryTypeId = enquiry.EnquiryTypeId,
                    ClientUserId = enquiry.ClientUserId,
                    EnquiryMessage = enquiry.EnquiryMessage,
                    AdminUserId = enquiry.AdminUserId,
                    EnquiryStatus = "Pending",
                    isDeleted = false
                };
                _dbContext.Enquiries.Add(newEnquiry);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new EnquiryDto
                {
                    ClientUserId = enquiry.ClientUserId,
                    EnquiryMessage = enquiry.EnquiryMessage,
                    EnquiryTypeId = enquiry.EnquiryTypeId,
                    EnquiryId = newEnquiry.EnquiryId,
                    AdminUserId = enquiry.AdminUserId,
                    EnquiryStatus = newEnquiry.EnquiryStatus

                });

            }
            else
            {

                var existingEnquiry = await (from e in _dbContext.Enquiries where e.EnquiryId == enquiry.EnquiryId && e.isDeleted == false select e).SingleOrDefaultAsync();
                if (existingEnquiry != null)
                {
                    existingEnquiry.ClientUserId = enquiry.ClientUserId;
                    existingEnquiry.EnquiryMessage = enquiry.EnquiryMessage;
                    existingEnquiry.EnquiryTypeId = enquiry.EnquiryTypeId;
                    existingEnquiry.AdminUserId = enquiry.AdminUserId;
                    _dbContext.Update(existingEnquiry);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new EnquiryDto
                    {
                        ClientUserId = enquiry.ClientUserId,
                        EnquiryMessage = enquiry.EnquiryMessage,
                        EnquiryTypeId = enquiry.EnquiryTypeId,
                        AdminUserId = enquiry.AdminUserId,
                        EnquiryId = existingEnquiry.EnquiryId
                    });
                }
            }
            return new EnquiryDto();

        }

        public async Task<List<GetEnquiryDto>> GetAllEnquiries()
        {
            return await (from e in _dbContext.Enquiries
                          join cu in _dbContext.Users on e.ClientUserId equals cu.UserId
                          join au in _dbContext.Users on e.ClientUserId equals au.UserId
                          join et in _dbContext.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                          where e.isDeleted == false
                          orderby e.EnquiryMessage
                          select new GetEnquiryDto
                          {
                              ClientUserId = e.ClientUserId,
                              EnquiryMessage = e.EnquiryMessage,
                              EnquiryTypeId = e.EnquiryTypeId,
                              AdminUserId = e.AdminUserId,
                              EnquiryId = e.EnquiryId,
                              EnquiryStatus = e.EnquiryStatus,
                              AdminName = $"{au.Initials} {au.Surname}",
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              EnquiryType = et.EnquiryTypeDescription
                          }).ToListAsync();
        }

        public async Task<GetEnquiryDto> GetEnquiryById(int enquiryId)
        {
            return await (from e in _dbContext.Enquiries
                          join cu in _dbContext.Users on e.ClientUserId equals cu.UserId
                          join au in _dbContext.Users on e.ClientUserId equals au.UserId
                          join et in _dbContext.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                          where e.isDeleted == false && e.EnquiryId == enquiryId
                          orderby e.EnquiryMessage
                          select new GetEnquiryDto
                          {
                              ClientUserId = e.ClientUserId,
                              EnquiryMessage = e.EnquiryMessage,
                              EnquiryTypeId = e.EnquiryTypeId,
                              AdminUserId = e.AdminUserId,
                              EnquiryId = e.EnquiryId,
                              EnquiryStatus = e.EnquiryStatus,
                              AdminName = $"{au.Initials} {au.Surname}",
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              EnquiryType = et.EnquiryTypeDescription
                          }).SingleOrDefaultAsync();
        }

        public async Task<bool> DeleteEnquiry(int enquiryId)
        {
            var enquiry = _dbContext.Enquiries.Find(enquiryId);
            if (enquiry != null)
            {
                enquiry.isDeleted = true;
                _dbContext.Enquiries.Update(enquiry);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<GetEnquiryDto>> GetAllAdminEnquiryById(int adminUserId)
        {
            return await (from e in _dbContext.Enquiries
                          join cu in _dbContext.Users on e.ClientUserId equals cu.UserId
                          join au in _dbContext.Users on e.ClientUserId equals au.UserId
                          join et in _dbContext.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                          where e.isDeleted == false && e.AdminUserId == adminUserId
                          orderby e.EnquiryMessage
                          select new GetEnquiryDto
                          {
                              ClientUserId = e.ClientUserId,
                              EnquiryMessage = e.EnquiryMessage,
                              EnquiryTypeId = e.EnquiryTypeId,
                              AdminUserId = e.AdminUserId,
                              EnquiryId = e.EnquiryId,
                              EnquiryStatus = e.EnquiryStatus,
                              AdminName = $"{au.Initials} {au.Surname}",
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              EnquiryType = et.EnquiryTypeDescription
                          }).ToListAsync();
        }
        public async Task<List<GetEnquiryDto>> GetAllAdminEnquiryByEnquiryTypeId(int adminUserId, int enquiryTypeId)
        {
            return await (from e in _dbContext.Enquiries
                          join cu in _dbContext.Users on e.ClientUserId equals cu.UserId
                          join au in _dbContext.Users on e.ClientUserId equals au.UserId
                          join et in _dbContext.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                          where e.isDeleted == false && e.AdminUserId == adminUserId && e.EnquiryTypeId == enquiryTypeId
                          orderby e.EnquiryMessage
                          select new GetEnquiryDto
                          {
                              ClientUserId = e.ClientUserId,
                              EnquiryMessage = e.EnquiryMessage,
                              EnquiryTypeId = e.EnquiryTypeId,
                              AdminUserId = e.AdminUserId,
                              EnquiryId = e.EnquiryId,
                              EnquiryStatus = e.EnquiryStatus,
                              AdminName = $"{au.Initials} {au.Surname}",
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              EnquiryType = et.EnquiryTypeDescription
                          }).ToListAsync();
        }
    }
    
}
