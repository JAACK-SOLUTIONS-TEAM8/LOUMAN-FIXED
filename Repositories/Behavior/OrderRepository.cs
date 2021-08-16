using Louman.AppDbContexts;
using Louman.Models.DTOs.Order;
using Louman.Models.DTOs.Product;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class OrderRepository : IOrderRepository
    {


        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<DeliveryTypeDto> AddDeliveryType(DeliveryTypeDto deliveryType)
        {
            if (deliveryType.DeliveryTypeId == 0)
            {
                var newDeliveryType = new DeliveryTypeEntity
                {
                    Description = deliveryType.Description,
                    isDeleted = false
                };
                _dbContext.DeliveryTypes.Add(newDeliveryType);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new DeliveryTypeDto
                {
                    DeliveryTypeId = newDeliveryType.DeliveryTypeId,
                    Description = deliveryType.Description
                });

            }
            else
            {

                var existingDeliveryType = await (from e in _dbContext.DeliveryTypes where e.DeliveryTypeId == deliveryType.DeliveryTypeId && e.isDeleted == false select e).SingleOrDefaultAsync();
                if (existingDeliveryType != null)
                {
                    existingDeliveryType.Description = deliveryType.Description;
                    _dbContext.Update(existingDeliveryType);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new DeliveryTypeDto
                    {
                        Description = existingDeliveryType.Description,
                        DeliveryTypeId = existingDeliveryType.DeliveryTypeId
                    });
                }
            }
            return new DeliveryTypeDto();

        }
        public async Task<bool> DeleteDeliveryType(int deliveryTypeId)
        {
            var deliveryType = _dbContext.DeliveryTypes.Find(deliveryTypeId);
            if (deliveryType != null)
            {
                deliveryType.isDeleted = true;
                _dbContext.DeliveryTypes.Update(deliveryType);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<DeliveryTypeDto>> GetAllDeliveryTypes()
        {
            return await (from d in _dbContext.DeliveryTypes
                          where d.isDeleted == false
                          orderby d.Description
                          select new DeliveryTypeDto
                          {
                              Description = d.Description,
                              DeliveryTypeId = d.DeliveryTypeId
                          }).ToListAsync();
        }



        }
    }

