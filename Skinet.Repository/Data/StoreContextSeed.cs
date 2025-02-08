using Skinet.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Skinet.Core.Entities.Order;

namespace Skinet.Repository.Data
{
    public  class StoreContextSeed
    {
        public static async Task SeedDataAsync(SkinetDbContext dbContext, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!dbContext.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Skinet.Repository/Data/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    if (brands?.Count() > 0)
                    {
                        foreach (var brand in brands)
                        {
                            dbContext.Set<ProductBrand>().Add(brand);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Skinet.Repository/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types?.Count() > 0)
                    {
                        foreach (var type in types)
                        {
                            dbContext.Set<ProductType>().Add(type);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }

                if (!dbContext.Products.Any())
                {
                    var productsData = File.ReadAllText("../Skinet.Repository/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                    if (products?.Count() > 0)
                    {
                        foreach (var product in products)
                        {
                            dbContext.Set<Product>().Add(product);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }


                if (!dbContext.DeliveryMethods.Any())
                {
                    var dmData = File.ReadAllText("../Skinet.Repository/Data/SeedData/delivery.json");
                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);
                    if (methods?.Count() > 0)
                    {
                        foreach (var method in methods)
                        {
                            dbContext.Set<DeliveryMethod>().Add(method);
                        }

                        await dbContext.SaveChangesAsync();
                    }
                }


            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex, "An error occurred during database migration.");
            }
        }

    }
}
