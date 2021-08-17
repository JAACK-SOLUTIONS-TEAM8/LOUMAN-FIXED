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

        [HttpPost("ProductSize/Add")]
        public async Task<IActionResult> AddNewProductSize(ProductSizeDto productSize)
        {
            var newProductSize = await _productReposiroty.AddProductSize(productSize);
            if (newProductSize != null)
                return Ok(new { ProductSize = newProductSize, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { ProductSize = newProductSize, StatusCode = StatusCodes.Status404NotFound });

        }

        [HttpGet("ProductSize/Delete/{id}")]
        public async Task<IActionResult> DeleteProductSize([FromRoute] int id)
        {
            var response = await _productReposiroty.DeleteProductSize(id);
            if (response != false)
                return Ok(new { Response = true, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Response = false, StatusCode = StatusCodes.Status404NotFound });

        }


        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productReposiroty.GetAllProduct();
            if (products != null)
                return Ok(new { Products = products, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { Products = products, StatusCode = StatusCodes.Status404NotFound });
        }

        [HttpPost("ProductType/Add")]
        public async Task<IActionResult> AddNewProductType(ProductTypeDto productType)
        {
            var newProductType = await _productReposiroty.AddProductType(productType);
            if (newProductType != null)
                return Ok(new { ProductType = newProductType, StatusCode = StatusCodes.Status200OK });
            return NotFound(new { ProductType = newProductType, StatusCode = StatusCodes.Status404NotFound });

        }









    }
}
