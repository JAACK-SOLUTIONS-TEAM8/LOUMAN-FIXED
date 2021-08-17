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

        public async Task<ProductSizeDto> AddProductSize(ProductSizeDto productSize)
        {
            if (productSize.ProductSizeId == 0)
            {
                var newProductSize = new ProductSizeEntity
                {
                    ProductSizeDescription = productSize.ProductSizeDescription,
                    isDeleted = false
                };
                _dbContext.ProductSizes.Add(newProductSize);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new ProductSizeDto
                {
                    ProductSizeId = newProductSize.ProductSizeId,
                    ProductSizeDescription = productSize.ProductSizeDescription
                });

            }
            else
            {

                var existingProductSize = await (from ps in _dbContext.ProductSizes where ps.ProductSizeId == productSize.ProductSizeId && ps.isDeleted == false select ps).SingleOrDefaultAsync();
                if (existingProductSize != null)
                {
                    existingProductSize.ProductSizeDescription = productSize.ProductSizeDescription;
                    _dbContext.Update(existingProductSize);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new ProductSizeDto
                    {
                        ProductSizeDescription = productSize.ProductSizeDescription,
                        ProductSizeId = productSize.ProductSizeId
                    });
                }
            }
            return new ProductSizeDto();

        }

        public async Task<bool> DeleteProductSize(int productSizeId)
        {
            var enquirySize = _dbContext.ProductSizes.Find(productSizeId);
            if (enquirySize != null)
            {
                enquirySize.isDeleted = true;
                _dbContext.ProductSizes.Update(enquirySize);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<GetStockProductDto>> GetAllProduct()
        {
            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false
                          orderby p.ProductName
                          select new GetStockProductDto
                          {
                              ProductName = p.ProductName,
                              Price = p.Price,
                              ProductSizeId = p.ProductSizeId,
                              ProductTypeId = p.ProductTypeId,
                              ProductId = p.ProductId,
                              ProductSizeDescription = ps.ProductSizeDescription,
                              ProductTypeName = pt.ProductTypeName,
                              ProductQuantity = s.ProductQuantity,
                              ProductImage = p.ProductImage,
                              StockId = s.StockId

                          }).ToListAsync();
        }



    }
}
