using Louman.Models.DTOs;
using Louman.Models.DTOs.Meeting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IMeetingRepository
    {
        Task<SlotDto> AddNewSlot(SlotDto slot);
        Task<List<SlotDto>> GetAllSlots();
        Task<List<SlotDto>> SearchSlotByDate(string date);
        Task<List<SlotDto>> SearchAdminSlotsByDate(int adminUserId, string date);
        Task<bool> DeleteSlot(int slotId);
        Task<SlotDto> GetBySlotId(int slotId);

        Task<BookedSlotDto> BookSlot(int slotId, int clientUserId);
        Task<List<BookedSlotDto>> GetAllBookedSlotByAdminId(int adminId);
        Task<List<BookedSlotDto>> SearchAllBookedSlotByAdminId(int userId, string date);
        Task<List<BookedSlotDto>> SearchAllBookedSlotByClient(int userId, string date);
        Task<List<BookedSlotDto>> GetAllBookedSlotByClientId(int clientId);
        Task<bool> CancelBooking(int slotId);

        Task<bool> DeleteBookedSlot(int clientId, int slotId);

    }
}
