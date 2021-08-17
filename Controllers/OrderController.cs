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
        [HttpGet("DeliveryType/{id}")]
        public async Task<IActionResult> GetDeliveryTypeById([FromRoute] int id)
        {
            var enquiryType = await _orderRepository.GetDeliveryTypeById(id);
            if (enquiryType != null)
                return Ok(new { DeliveryType = enquiryType, StatusCode = StatusCodes.Status200OK });
            return Ok(new { DeliveryType = enquiryType, StatusCode = StatusCodes.Status404NotFound });

        }
        [HttpPost("DeliveryType/Add")]
        public async Task<IActionResult> AddDeliveryType([FromBody] DeliveryTypeDto deliveryType)
        {
            var enquiry = await _orderRepository.AddDeliveryType(deliveryType);
            if (enquiry != null)
                return Ok(new { DeliveryType = enquiry, StatusCode = StatusCodes.Status200OK });
            return Ok(new { DeliveryType = enquiry, StatusCode = StatusCodes.Status400BadRequest });

        }
        [HttpGet("DeliveryType/Delete/{id}")]
        public async Task<IActionResult> DelteDeliveryType([FromRoute] int id)
        {
            var response = await _orderRepository.DeleteDeliveryType(id);
            if (response != false)
                return Ok(new { DeliveryTypes = response, StatusCode = StatusCodes.Status200OK });
            return Ok(new { DeliveryTypes = response, StatusCode = StatusCodes.Status400BadRequest });

        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddOrder([FromBody] OrderDto newOrdre)
        {
            var order = await _orderRepository.AddOrder(newOrdre);
            if (order != null)
                return Ok(new { Order = order, StatusCode = StatusCodes.Status200OK });
            return Ok(new { Order = order, StatusCode = StatusCodes.Status400BadRequest });

        }



    }
}
