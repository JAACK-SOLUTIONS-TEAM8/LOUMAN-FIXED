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
        Task<ProductDto> AddProduct(UserProduct product);
        Task<ProductSizeDto> AddProductSize(ProductSizeDto productSize);
        Task<bool> DeleteProductSize(int productSizeId);
        Task<List<GetStockProductDto>> GetAllProduct();
        Task<List<GetStockProductDto>> SearchProductByName(string name);
        Task<GetStockProductDto> GetProductById(int productId);
        Task<GetStockProductDto> GetStockProductById(int stockId);
        Task<int> GetProductQuantityInStock(int productId);
        Task<bool> DeleteProduct(int productId);
        Task<GetStockProductDto> WireOffStock(StockDto stock);
        Task<GetStockProductDto> CompleteStock(StockDto stock);

        Task<List<ProductSizeDto>> GetAllProductSize();
        Task<ProductSizeDto> GetProductSizeById(int productSizeId);
        Task<List<GetProductDto>> GetStockMonthlyReport(string dateInfo);


    }
}
