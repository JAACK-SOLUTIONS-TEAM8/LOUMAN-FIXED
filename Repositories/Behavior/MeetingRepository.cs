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
        public async Task<List<SlotDto>> SearchSlotByDate(string date)
        {
            return await (from s in _dbContext.Slots
                          join u in _dbContext.Users on s.AdminUserId equals u.UserId
                          where s.isDeleted == false && (date == null || s.Date.Date == Convert.ToDateTime(date).Date)
                          select new SlotDto
                          {
                              Date = s.Date,
                              SlotId = s.SlotId,
                              AdminUserId = u.UserId,
                              isBooked = s.isBooked,
                              EndTime = s.StartTime.ToString("F"),
                              StartTime = s.EndTime.ToString("F")

                          }).ToListAsync();
        }
        public async Task<List<SlotDto>> SearchAdminSlotsByDate(int adminUserId, string date)
        {
            return await (from s in _dbContext.Slots
                          join u in _dbContext.Users on s.AdminUserId equals u.UserId
                          where s.isDeleted == false && (string.IsNullOrEmpty(date) || (s.Date.Date == Convert.ToDateTime(date).Date) && s.AdminUserId == adminUserId)
                          select new SlotDto
                          {
                              Date = s.Date,
                              SlotId = s.SlotId,
                              isBooked = s.isBooked,
                              AdminUserId = u.UserId,
                              EndTime = s.StartTime.ToString("F"),
                              StartTime = s.EndTime.ToString("F")

                          }).ToListAsync();
        }
        public async Task<List<BookedSlotDto>> GetAllBookedSlotByAdminId(int adminUserId)
        {
            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && bs.AdminUserId == adminUserId && s.isBooked == true && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Initials} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).ToListAsync();
        }

        public async Task<BookedSlotDto> BookSlot(int slotId, int clientUserId)
        {
            var slot = await (from s in _dbContext.Slots
                              join u in _dbContext.Users on s.AdminUserId equals u.UserId
                              where s.SlotId == slotId
                              select
        new SlotDto
        {
            Date = s.Date,
            SlotId = s.SlotId,
            isBooked = s.isBooked,
            AdminUserId = u.UserId,
            EndTime = s.EndTime.ToString("F"),
            StartTime = s.StartTime.ToString("F")
        }).SingleOrDefaultAsync();

            var bookedSlotEntity = new BookedSlotEntity
            {
                BookingTime = DateTime.Now,
                AdminUserId = slot.AdminUserId,
                ClientUserId = clientUserId,
                SlotId = slotId,
                isDeleted = false


            };

            _dbContext.BookedSlots.Add(bookedSlotEntity);
            await _dbContext.SaveChangesAsync();

            var slotEntity = _dbContext.Slots.Find(slotId);
            slotEntity.isBooked = true;

            _dbContext.Slots.Update(slotEntity);
            _dbContext.SaveChanges();

            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && bs.BookedSlotId == bookedSlotEntity.BookedSlotId && s.isBooked == true && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Initials} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).FirstOrDefaultAsync();

        }

      

        public async Task<List<BookedSlotDto>> GetAllBookedSlotByClientId(int clientUserId)
        {
            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && bs.ClientUserId == clientUserId && s.isBooked == true && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Initials} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Initials} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).ToListAsync();
        }

        public async Task<bool> DeleteBookedSlot(int clientId, int slotId)
        {
            var bookingTime = await (
                                    from bs in _dbContext.BookedSlots
                                    join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                                    where s.isDeleted == false
                                    select bs.BookingTime).SingleOrDefaultAsync();
            if (bookingTime.Subtract(DateTime.Now).Hours < 24)
            {
                var slot = _dbContext.Slots.Find(slotId);
                slot.isDeleted = true;
                _dbContext.Slots.Update(slot);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CancelBooking(int slotId)
        {
            var bookedSlot = await (
                                    from bs in _dbContext.BookedSlots
                                    join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                                    where s.isDeleted == false && s.SlotId == slotId
                                    select bs).SingleOrDefaultAsync();
            if (bookedSlot.BookingTime.Subtract(DateTime.Now).Hours < 24)
            {
                var slot = _dbContext.Slots.Find(slotId);
                slot.isBooked = false;
                _dbContext.Slots.Update(slot);
                await _dbContext.SaveChangesAsync();

                var bookedSlotEntity = await _dbContext.BookedSlots.FindAsync(bookedSlot.BookedSlotId);
                bookedSlotEntity.isDeleted = true;
                _dbContext.BookedSlots.Update(bookedSlotEntity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
