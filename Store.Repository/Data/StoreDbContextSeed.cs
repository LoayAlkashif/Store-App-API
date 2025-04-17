using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Repository.Data.Context;

namespace Store.Repository.Data
{
    public class StoreDbContextSeed
    {


        public async static Task SeedAsync(StoreDbContext _context)
        {
            if(_context.Brands.Count() == 0)
            {
                // brand
                //1. read data from json file

                var brandsData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\brands.json");
                //2. convert json string to List<T>

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                //3. seed data to database
                if (brands is not null && brands.Count > 0)
                {
                    await _context.Brands.AddRangeAsync(brands);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.Types.Count() == 0)
            {
                // Types
                //1. read data from json file

                var typeData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\types.json");
                //2. convert json string to List<T>

                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                //3. seed data to database
                if (types is not null && types.Count > 0)
                {
                    await _context.Types.AddRangeAsync(types);
                    await _context.SaveChangesAsync();
                }
            }


            if (_context.Products.Count() == 0)
            {
                // Products
                //1. read data from json file

                var productData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\products.json");
                //2. convert json string to List<T>

                var products = JsonSerializer.Deserialize<List<Product>>(productData);

                //3. seed data to database
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.DeliveryMethods.Count() == 0)
            {
                // Products
                //1. read data from json file

                var deliveryData = File.ReadAllText(@"..\Store.Repository\Data\DataSeed\delivery.json");
                //2. convert json string to List<T>

                var deliveries = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                //3. seed data to database
                if (deliveries is not null && deliveries.Count > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(deliveries);
                    await _context.SaveChangesAsync();
                }
            }


        }
    }
}
