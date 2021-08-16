using Louman.Models.DTOs.Order;
using Louman.Repositories;
using Louman.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet("DeliveryType/All")]
        public async Task<IActionResult> GetAllDeliveryTypes()
        {
            var enquiryTypes = await _orderRepository.GetAllDeliveryTypes();
            if (enquiryTypes != null)
                return Ok(new { DeliveryTypes = enquiryTypes, StatusCode = StatusCodes.Status200OK });
            return Ok(new { DeliveryTypes = enquiryTypes, StatusCode = StatusCodes.Status404NotFound });

        }
    }
}
