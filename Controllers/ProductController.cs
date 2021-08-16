using Louman.Models.DTOs.Product;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productReposiroty;

        public ProductController(IProductRepository productReposiroty)
        {
            _productReposiroty = productReposiroty;
        }

        

      
        [HttpPost("Add")]
        public async Task<IActionResult> AddNewProduct(ProductDto product)
        {
            var newProduct =await  _productReposiroty.AddProduct(product);
            if (product != null)
                return Ok(new { Product = newProduct, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Product = newProduct, StatusCode = StatusCodes.Status404NotFound });

        }

       




        

      

    }
}
