using Louman.Models.DTOs.Order;
using Louman.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IOrderRepository
    {
        Task<DeliveryTypeDto> AddDeliveryType(DeliveryTypeDto deliveryType);
        Task<List<DeliveryTypeDto>> GetAllDeliveryTypes();
        Task<DeliveryTypeDto> GetDeliveryTypeById(int deliveryTypeId);
        Task<bool> DeleteDeliveryType(int deliveryTypeId);

        Task<GetOrderDto> AddOrder(OrderDto order);
        Task<List<GetOrderDto>> GetAllClientOrders();
        Task<ClientOrderDto> GetAllClientOrderById(int orderId);
        Task<bool> CompleteOrder(int orderId);
        Task<List<ProductQuantityDto>> GetMonthlyReport(string dateInfo);

        Task<List<GetOrderDto>> GetAllClientOrdersByClientUserId(int clientUserId);



    }
}
