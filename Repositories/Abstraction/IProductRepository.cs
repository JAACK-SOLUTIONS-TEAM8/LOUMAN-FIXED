using Louman.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IProductRepository
    {
        

        Task<ProductDto> AddProduct(ProductDto product);
        Task<ProductSizeDto> AddProductSize(ProductSizeDto productSize);


    }
}
