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
        public async Task<SlotDto> AddNewSlot(SlotDto slot)
        {
            if (slot.SlotId == 0)
            {
                var newSlot = new SlotEntity
                {
                    Date = slot.Date,
                    StartTime = Convert.ToDateTime(slot.StartTime),
                    EndTime = Convert.ToDateTime(slot.EndTime),
                    isBooked = false,
                    AdminUserId = slot.AdminUserId,
                    isDeleted = false
                };
                _dbContext.Slots.Add(newSlot);
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(new SlotDto
                {
                    Date = slot.Date,
                    SlotId = newSlot.SlotId,
                    AdminUserId = slot.AdminUserId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    isBooked = false
                });

            }
            else
            {

                var existingSlot = (from s in _dbContext.Slots where s.SlotId == slot.SlotId && s.isDeleted == false select s).SingleOrDefault();
                if (existingSlot != null)
                {
                    existingSlot.StartTime = Convert.ToDateTime(slot.StartTime);
                    existingSlot.EndTime = Convert.ToDateTime(slot.EndTime);

                    _dbContext.Update(existingSlot);
                    await _dbContext.SaveChangesAsync();



                    return await Task.FromResult(new SlotDto
                    {
                        Date = slot.Date,
                        SlotId = slot.SlotId,
                        AdminUserId = slot.AdminUserId,
                        StartTime = slot.StartTime,
                        EndTime = slot.EndTime,
                        isBooked = slot.isBooked
                    });
                }
            }
            return null;

        }

        public async Task<List<SlotDto>> GetAllSlots()
        {
            return await (from s in _dbContext.Slots
                          join u in _dbContext.Users on s.AdminUserId equals u.UserId
                          where s.isDeleted == false && s.isBooked == false
                          select new SlotDto
                          {
                              Date = s.Date,
                              SlotId = s.SlotId,
                              isBooked = s.isBooked,
                              AdminUserId = u.UserId,
                              StartTime = s.StartTime.ToString("F"),
                              EndTime = s.EndTime.ToString("F")

                          }).ToListAsync();
        }

        public async Task<SlotDto> GetBySlotId(int slotId)
        {
            return await (from s in _dbContext.Slots
                          join u in _dbContext.Users on s.AdminUserId equals u.UserId
                          where s.isDeleted == false && s.SlotId == slotId
                          select new SlotDto
                          {
                              Date = s.Date,
                              SlotId = s.SlotId,
                              isBooked = s.isBooked,
                              AdminUserId = u.UserId,
                              EndTime = s.StartTime.ToString("F"),
                              StartTime = s.EndTime.ToString("F")

                          }).SingleOrDefaultAsync();

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


            var auditEntity = new AuditEntity
            {
                Date = DateTime.Now,
                UserId = clientUserId,
                Operation = $"Slot is Booked!"
            };

            await _dbContext.Audits.AddAsync(auditEntity);
            await _dbContext.SaveChangesAsync();

            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && bs.BookedSlotId == bookedSlotEntity.BookedSlotId && s.isBooked == true && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Name} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Name} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).FirstOrDefaultAsync();

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
                              AdminName = $"{au.Name} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Name} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId,
                              ClientPhone=cu.CellNumber
                          }).ToListAsync();
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
                              AdminName = $"{au.Name} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Name} {cu.Surname}",
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
                                    where s.isDeleted == false && bs.isDeleted == false && s.SlotId == slotId
                                    select bs).SingleOrDefaultAsync();

            var timeDiff = bookedSlot.BookingTime.Subtract(DateTime.Now);

            if (timeDiff.TotalHours > 24)
            {
                var slot = _dbContext.Slots.Find(slotId);
                if(slot.Date< DateTime.Now.Date)
                {
                    slot.isDeleted = true;
                }
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

        public async Task<List<BookedSlotDto>> SearchAllBookedSlotByAdminId(int userId, string date)
        {
            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && (string.IsNullOrEmpty(date) || (s.Date.Date == Convert.ToDateTime(date).Date) && s.AdminUserId == userId) && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Name} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Name} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).ToListAsync();
        }
        public async Task<List<BookedSlotDto>> SearchAllBookedSlotByClient(int userId, string date)
        {
            return await (from bs in _dbContext.BookedSlots
                          join s in _dbContext.Slots on bs.SlotId equals s.SlotId
                          join au in _dbContext.Users on bs.AdminUserId equals au.UserId
                          join cu in _dbContext.Users on bs.ClientUserId equals cu.UserId
                          where s.isDeleted == false && (string.IsNullOrEmpty(date) || (s.Date.Date == Convert.ToDateTime(date).Date) && bs.ClientUserId == userId) && bs.isDeleted == false
                          select new BookedSlotDto
                          {
                              AdminUserId = bs.AdminUserId,
                              AdminName = $"{au.Name} {au.Surname}",
                              BookedSlotId = bs.BookedSlotId,
                              ClientName = $"{cu.Name} {cu.Surname}",
                              ClientUserId = bs.ClientUserId,
                              Date = s.Date,
                              StartTime = s.StartTime,
                              EndTime = s.EndTime,
                              SlotId = s.SlotId
                          }).ToListAsync();
        }
    }
}
