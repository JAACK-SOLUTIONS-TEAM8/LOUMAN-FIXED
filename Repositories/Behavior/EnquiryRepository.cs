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

    }
}
