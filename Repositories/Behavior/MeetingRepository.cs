using Louman.AppDbContexts;
using Louman.Models.DTOs;
using Louman.Models.DTOs.Meeting;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Behavior
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly AppDbContext _dbContext;

        public MeetingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<List<SlotDto>> GetAllSlots()
        {
            return await (from s in _dbContext.Slots
                         join u in _dbContext.Users on s.AdminUserId equals u.UserId
                         where s.isDeleted == false && s.isBooked==false
                         select new SlotDto
                         {
                             Date=s.Date,
                            SlotId=s.SlotId,
                            isBooked=s.isBooked,
                            AdminUserId=u.UserId,
                            StartTime=s.StartTime.ToString("F"),
                            EndTime=s.EndTime.ToString("F")

                         }).ToListAsync();
        }

        public async Task<bool> DeleteSlot(int slotId)
        {
            var existingSlot = _dbContext.Slots.Find(slotId);
            if (existingSlot != null)
            {
                existingSlot.isDeleted = true;
                _dbContext.Slots.Update(existingSlot);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
