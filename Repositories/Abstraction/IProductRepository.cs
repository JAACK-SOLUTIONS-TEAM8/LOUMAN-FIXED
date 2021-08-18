using Louman.Models.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories.Abstraction
{
    public interface IProductRepository
    {

        Task<ProductTypeDto> AddProductType(ProductTypeDto productType);
        Task<List<ProductTypeDto>> GetAllProductTypes();
        Task<ProductTypeDto> GetProductTypeById(int productTypeId);
        Task<bool> DeleteProductType(int enquiryType);
        Task<ProductDto> AddProduct(ProductDto product);
        Task<ProductSizeDto> AddProductSize(ProductSizeDto productSize);
        Task<bool> DeleteProductSize(int productSizeId);
        Task<List<GetStockProductDto>> GetAllProduct();
        Task<GetStockProductDto> GetProductById(int productId);


    }
}
