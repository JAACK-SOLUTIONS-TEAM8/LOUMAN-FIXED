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
    }

