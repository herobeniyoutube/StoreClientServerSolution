using Microsoft.EntityFrameworkCore;
using StoreServer.Entities;
using StoreServer.Models.EFCoreEntitiesCopies;

namespace StoreServer.Services
{
    /// <summary>
    /// Service provides various methods for managing products in the system
    /// </summary>
    public class ProductsService
    {
        StoreContext db;
        public ProductsService(StoreContext db)
        {
            this.db = db;
        }
        /// <summary>
        /// Gets all products list from server
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var list = await db.Products.ToListAsync();
            return list;
        }
        /// <summary>
        /// Adds new product to the system
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<Product> AddNewProductAsync(Product product)
        {
            ProductEntity productEntity = new ProductEntity()
            { 
            ProductName = product.ProductName,
            Price = product.Price
            };
            await db.Products.AddAsync(productEntity);
            await db.SaveChangesAsync();
            return product;
        }
    }
}
