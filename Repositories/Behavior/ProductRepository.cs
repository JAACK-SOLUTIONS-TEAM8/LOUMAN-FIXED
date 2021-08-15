using Louman.AppDbContexts;
using Louman.Models.DTOs.Product;
using Louman.Models.Entities;
using Louman.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Louman.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<ProductDto> AddProduct(ProductDto product)
        {
            if (product.ProductId == 0)
            {
                var newProduct = new ProductEntity
                {
                    ProductName = product.ProductName,
                    Price=product.Price,
                    ProductSizeId=product.ProductSizeId,
                    ProductTypeId=product.ProductTypeId,
                    ProductImage=product.ProductImage,
                    isDeleted = false
                };
                _dbContext.Products.Add(newProduct);
                await _dbContext.SaveChangesAsync();

                var stockEntity = new StockEntity
                {
                    ProductId=newProduct.ProductId,
                    ProductQuantity=0,
                    Date=DateTime.Now,
                    isDeleted=false
                };

                await _dbContext.Stocks.AddAsync(stockEntity);
                await _dbContext.SaveChangesAsync();
                return await Task.FromResult(new ProductDto
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductSizeId = product.ProductSizeId,
                    ProductTypeId = product.ProductTypeId,
                    ProductImage = product.ProductImage,
                    ProductId = newProduct.ProductId
                });

            }
            else
            {

                var existingProduct = await(from p in _dbContext.Products where p.ProductId == product.ProductId && p.isDeleted == false select p).SingleOrDefaultAsync();
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.Price = product.Price;
                    existingProduct.ProductSizeId = product.ProductSizeId;
                    existingProduct.ProductImage = product.ProductImage;

                    existingProduct.ProductTypeId = product.ProductTypeId;
                    
                    _dbContext.Update(existingProduct);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new ProductDto
                    {
                        ProductName = product.ProductName,
                        Price = product.Price,
                        ProductSizeId = product.ProductSizeId,
                        ProductTypeId = product.ProductTypeId,
                        ProductId = product.ProductId,
                        ProductImage=product.ProductImage
                    });
                }
            }
            return new ProductDto();

        }
        
      

        
    }
}
