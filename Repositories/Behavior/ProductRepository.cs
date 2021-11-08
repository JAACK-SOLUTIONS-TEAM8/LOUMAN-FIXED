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


        public async Task<ProductDto> AddProduct(UserProduct userProduct)
        {
            var product = userProduct.Product;
            if (product.ProductId == 0)
            {
                var newProduct = new ProductEntity
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductSizeId = product.ProductSizeId,
                    ProductTypeId = product.ProductTypeId,
                    ProductImage = product.ProductImage,
                    isVatIncluded = product.IsVatIncluded,
                    VatAmount = product.IsVatIncluded.Value ? product.Price * 0.15M : 0,
                    isDeleted = false,
                    Date = DateTime.Now
                };
                _dbContext.Products.Add(newProduct);
                await _dbContext.SaveChangesAsync();

                var stockEntity = new StockEntity
                {
                    ProductId = newProduct.ProductId,
                    ProductQuantity = 0,
                    Date = DateTime.Now,
                    isDeleted = false
                };

                await _dbContext.Stocks.AddAsync(stockEntity);
                await _dbContext.SaveChangesAsync();

                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = userProduct.UserId,
                    Operation = $"Product:{product.ProductName} is added to the system"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(new ProductDto
                {
                    ProductName = product.ProductName,
                    Price = product.Price,
                    ProductSizeId = product.ProductSizeId,
                    ProductTypeId = product.ProductTypeId,
                    ProductImage = product.ProductImage,
                    ProductId = newProduct.ProductId,
                    IsVatIncluded = newProduct.isVatIncluded

                });

            }
            else
            {

                var existingProduct = await (from p in _dbContext.Products where p.ProductId == product.ProductId && p.isDeleted == false select p).SingleOrDefaultAsync();
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.Price = product.Price;
                    existingProduct.ProductSizeId = product.ProductSizeId;
                    existingProduct.ProductImage = product.ProductImage;
                    existingProduct.isVatIncluded = product.IsVatIncluded;
                    existingProduct.VatAmount = product.IsVatIncluded.Value ? product.Price * 0.15M : 0;
                    existingProduct.ProductTypeId = product.ProductTypeId;

                    _dbContext.Update(existingProduct);
                    await _dbContext.SaveChangesAsync();

                    var auditEntity = new AuditEntity
                    {
                        Date = DateTime.Now,
                        UserId = 1,
                        Operation = $"Product:{product.ProductName} is updated in the system"
                    };

                    await _dbContext.Audits.AddAsync(auditEntity);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new ProductDto
                    {
                        ProductName = product.ProductName,
                        Price = product.Price,
                        ProductSizeId = product.ProductSizeId,
                        ProductTypeId = product.ProductTypeId,
                        ProductId = product.ProductId,
                        IsVatIncluded = product.IsVatIncluded,
                        ProductImage = product.ProductImage
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
                              StockId = s.StockId,
                              IsVatIncluded = p.isVatIncluded,
                              VatAmount = p.VatAmount

                          }).ToListAsync();
        }

        public async Task<ProductTypeDto> AddProductType(ProductTypeDto productType)
        {
            if (productType.ProductTypeId == 0)
            {
                var newProductType = new ProductTypeEntity
                {
                    ProductTypeName = productType.ProductTypeName,
                    isDeleted = false
                };
                await _dbContext.ProductTypes.AddAsync(newProductType);
                await _dbContext.SaveChangesAsync();


                return await Task.FromResult(new ProductTypeDto
                {
                    ProductTypeId = newProductType.ProductTypeId,
                    ProductTypeName = productType.ProductTypeName
                });

            }
            else
            {

                var existingProductType = await (from pt in _dbContext.ProductTypes where pt.ProductTypeId == productType.ProductTypeId && pt.isDeleted == false select pt).SingleOrDefaultAsync();
                if (existingProductType != null)
                {
                    existingProductType.ProductTypeName = productType.ProductTypeName;
                    _dbContext.Update(existingProductType);
                    await _dbContext.SaveChangesAsync();

                    return await Task.FromResult(new ProductTypeDto
                    {
                        ProductTypeName = productType.ProductTypeName,
                        ProductTypeId = productType.ProductTypeId
                    });
                }
            }
            return new ProductTypeDto();

        }
        public async Task<List<ProductTypeDto>> GetAllProductTypes()
        {
            return await (from pt in _dbContext.ProductTypes
                          where pt.isDeleted == false
                          orderby pt.ProductTypeName
                          select new ProductTypeDto
                          {
                              ProductTypeId = pt.ProductTypeId,
                              ProductTypeName = pt.ProductTypeName
                          }).ToListAsync();
        }
        public async Task<ProductTypeDto> GetProductTypeById(int productTypeId)
        {
            return await (from pt in _dbContext.ProductTypes
                          where pt.isDeleted == false && pt.ProductTypeId == productTypeId
                          orderby pt.ProductTypeName
                          select new ProductTypeDto
                          {
                              ProductTypeId = pt.ProductTypeId,
                              ProductTypeName = pt.ProductTypeName
                          }).SingleOrDefaultAsync();
        }
        public async Task<bool> DeleteProductType(int productTypeId)
        {
            var productType = _dbContext.ProductTypes.Find(productTypeId);
            if (productType != null)
            {
                productType.isDeleted = true;
                _dbContext.ProductTypes.Update(productType);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<GetStockProductDto> GetProductById(int productId)
        {
            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && p.ProductId == productId
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
                              StockId = s.StockId,
                              IsVatIncluded = p.isVatIncluded,
                              VatAmount = p.VatAmount

                          }).SingleOrDefaultAsync();
        }

        public async Task<int> GetProductQuantityInStock(int productId)
        {
            var quantity = (await (_dbContext.Stocks.Where(stock => stock.ProductId == productId)).SingleOrDefaultAsync()).ProductQuantity;
            return quantity;

        }
        public async Task<List<GetProductDto>> GetStockMonthlyReport(string dateInfo)
        {
            var date = DateTime.Parse(dateInfo);

            return await (from p in _dbContext.Products
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join pt in _dbContext.ProductTypes on p.ProductSizeId equals pt.ProductTypeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where s.Date.Date.Month == date.Month && s.Date.Date.Year == date.Year
                          select new GetProductDto
                          {
                              ProductId = p.ProductId,
                              ProductSizeDescription = ps.ProductSizeDescription,
                              ProductName = p.ProductName,
                              Price = p.Price,
                              ProductSizeId = p.ProductSizeId,
                              ProductTypeId = p.ProductTypeId,
                              ProductTypeName = pt.ProductTypeName,
                              Quantity = s.ProductQuantity
                          }).ToListAsync();


        }

        public async Task<bool> DeleteProduct(int productId)
        {
            var product = _dbContext.Products.Find(productId);

            var productsInActiveOrders = (from p in _dbContext.Products
                                          join ol in _dbContext.OrderLines on p.ProductId equals ol.ProductId
                                          join o in _dbContext.Orders on ol.OrderId equals o.OrderId
                                          where o.OrderStatus == "Pending" && o.isDeleted == false
                                          select new { productId = p.ProductId }).ToList();

            if (product != null && !productsInActiveOrders.Any(p => p.productId == productId))
            {
                product.isDeleted = true;
                _dbContext.Products.Update(product);
                await _dbContext.SaveChangesAsync();



                var auditEntity = new AuditEntity
                {
                    Date = DateTime.Now,
                    UserId = 1,
                    Operation = $"Product:{product.ProductName} is deleted From the system"
                };

                await _dbContext.Audits.AddAsync(auditEntity);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public async Task<GetStockProductDto> WireOffStock(StockDto stock)
        {

            var stockEntity = await _dbContext.Stocks.FindAsync(stock.StockId);
            stockEntity.ProductQuantity = stockEntity.ProductQuantity - stock.ProductQuantity;
            _dbContext.Stocks.Update(stockEntity);
            await _dbContext.SaveChangesAsync();



            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && s.StockId == stock.StockId
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
                              StockId = s.StockId,
                              ProductImage = p.ProductImage,
                              IsVatIncluded = p.isVatIncluded,
                              VatAmount = p.VatAmount


                          }).SingleOrDefaultAsync();



        }

        public async Task<GetStockProductDto> CompleteStock(StockDto stock)
        {

            var stockEntity = await _dbContext.Stocks.FindAsync(stock.StockId);
            stockEntity.ProductQuantity = stockEntity.ProductQuantity + stock.ProductQuantity;
            _dbContext.Stocks.Update(stockEntity);
            await _dbContext.SaveChangesAsync();



            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && s.StockId == stock.StockId
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
                              StockId = s.StockId,
                              ProductImage = p.ProductImage,
                              IsVatIncluded = p.isVatIncluded,
                              VatAmount = p.VatAmount

                          }).SingleOrDefaultAsync();



        }
        public async Task<GetStockProductDto> CapturefStock(StockDto stock)
        {
            var stockEntity = await _dbContext.Stocks.FindAsync(stock.StockId);
            stockEntity.ProductQuantity = stock.ProductQuantity;
            _dbContext.Stocks.Update(stockEntity);
            await _dbContext.SaveChangesAsync();



            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && s.StockId == stock.StockId
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
                              StockId = s.StockId,
                              ProductImage = p.ProductImage,
                              IsVatIncluded = p.isVatIncluded,
                              VatAmount = p.VatAmount

                          }).SingleOrDefaultAsync();
        }


        public async Task<GetStockProductDto> GetStockProductById(int stockId)
        {
            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && s.StockId == stockId
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
                              StockId = s.StockId,
                              ProductImage = p.ProductImage

                          }).SingleOrDefaultAsync();

        }

        public async Task<List<ProductSizeDto>> GetAllProductSize()
        {
            return await (from ps in _dbContext.ProductSizes
                          where ps.isDeleted == false
                          orderby ps.ProductSizeDescription
                          select new ProductSizeDto
                          {
                              ProductSizeId = ps.ProductSizeId,
                              ProductSizeDescription = ps.ProductSizeDescription
                          }).ToListAsync();

        }

        public async Task<ProductSizeDto> GetProductSizeById(int productSizeId)
        {
            return await (from ps in _dbContext.ProductSizes
                          where ps.isDeleted == false && ps.ProductSizeId == productSizeId
                          orderby ps.ProductSizeDescription
                          select new ProductSizeDto
                          {
                              ProductSizeId = ps.ProductSizeId,
                              ProductSizeDescription = ps.ProductSizeDescription
                          }).SingleOrDefaultAsync();
        }

        public async Task<List<GetStockProductDto>> SearchProductByName(string name)
        {
            return await (from p in _dbContext.Products
                          join pt in _dbContext.ProductTypes on p.ProductTypeId equals pt.ProductTypeId
                          join ps in _dbContext.ProductSizes on p.ProductSizeId equals ps.ProductSizeId
                          join s in _dbContext.Stocks on p.ProductId equals s.ProductId
                          where p.isDeleted == false && (p.ProductName.StartsWith(name) || p.ProductName.Contains(name))
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
        public async Task<MonthlySoldProductDto> GetSaleAmountForEachProduct(DateTime date)
        {
            var productsInStock = await (from sp in _dbContext.Stocks
                                         join p in _dbContext.Products on sp.ProductId equals p.ProductId
                                         where sp.isDeleted == false && p.isDeleted == false
                                         select new ProductInStock
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName,
                                             Price = p.Price
                                         }).ToListAsync();

            var months = await _dbContext.Months.Where(month => month.MonthId == date.Month).FirstOrDefaultAsync();
            var productsSold = await (from o in _dbContext.Orders
                                      join ol in _dbContext.OrderLines on o.OrderId equals ol.OrderId
                                      where o.isDeleted == false && o.OrderStatus == "Delivered" && o.CreatedDate.Value.Month == date.Month && o.CreatedDate.Value.Year == date.Year
                                      select new
                                      {
                                          ProductId = ol.ProductId,
                                          SoldQuantity = ol.Quantity ?? 0
                                      }).ToListAsync();

            var soldProductDetail = new List<SoldProductDto>();
            decimal sum = 0;
            foreach (var product in productsInStock)
            {

                var totalSold = productsSold.Where(p => p.ProductId == product.ProductId).Sum(p => p.SoldQuantity * product.Price);
                sum += totalSold;
                var quantitySold = productsSold.Where(p => p.ProductId == product.ProductId).Sum(p => p.SoldQuantity);
                soldProductDetail.Add(new SoldProductDto { ProductId = product.ProductId, ProductName = product.ProductName, TotalSoldPrice = totalSold, QuantitySold = quantitySold });
            }

            return await Task.FromResult(new MonthlySoldProductDto { SoldProducts = soldProductDetail, MonthId = months.MonthId, MonthName = months.MonthName, MonthAverage = sum / productsInStock.Count() });
        }
        public async Task<List<MonthlySoldProductDto>> GetSixMonthSaleAmountForEachProduct()
        {

            var mons = new Dictionary<int, int>();
            for (int i = 0; i < 6; i++)
            {
                if ((DateTime.Now.Date.Month - i) >= 0)
                    mons.Add(DateTime.Now.Date.Month - i, DateTime.Now.Date.Year);
                else
                    mons.Add(12 - i, DateTime.Now.Date.Year - 1);

            }

            var productsInStock = await (from sp in _dbContext.Stocks
                                         join p in _dbContext.Products on sp.ProductId equals p.ProductId
                                         where sp.isDeleted == false && p.isDeleted == false
                                         select new ProductInStock
                                         {
                                             ProductId = p.ProductId,
                                             ProductName = p.ProductName,
                                             Price = p.Price
                                         }).ToListAsync();

            var months = await _dbContext.Months.Where(month => mons.Keys.Contains(month.MonthId)).ToListAsync();

            var monthlySoldProducts = new List<MonthlySoldProductDto>();

            foreach (var month in months)
            {
                var productsSold = await (from o in _dbContext.Orders
                                          join ol in _dbContext.OrderLines on o.OrderId equals ol.OrderId
                                          where o.isDeleted == false && o.OrderStatus == "Delivered" && o.CreatedDate.Value.Month == month.MonthId
                                          select new SoldProductQuantityDto
                                          {
                                              ProductId = ol.ProductId,
                                              SoldQuantity = ol.Quantity.HasValue ? ol.Quantity.Value : 0
                                          }).ToListAsync();
                var soldProductDetail = new List<SoldProductDto>();
                decimal sum = 0;

                foreach (var product in productsInStock)
                {

                    var totalSold = productsSold.Where(p => p.ProductId == product.ProductId).Sum(p => p.SoldQuantity * product.Price);
                    sum += totalSold;

                    var quantitySold = productsSold.Where(p => p.ProductId == product.ProductId).Sum(p => p.SoldQuantity);
                    soldProductDetail.Add(new SoldProductDto { ProductId = product.ProductId, ProductName = product.ProductName, TotalSoldPrice = totalSold, QuantitySold = quantitySold, });
                }
                var average = sum / soldProductDetail.Count();
                monthlySoldProducts.Add(new MonthlySoldProductDto { MonthId = month.MonthId, MonthName = month.MonthName, SoldProducts = soldProductDetail, MonthAverage = average });
            }




            return await Task.FromResult(monthlySoldProducts);
        }

    }
}
