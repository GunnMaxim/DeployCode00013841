using Microsoft.EntityFrameworkCore;
using sem3Tuesday3.DbContexts;
using sem3Tuesday3.Models;
using System.Collections.Generic;
using System.Linq;

namespace sem3Tuesday3.Repository
{
    public class ProductRepository : IProductRepository

    {
       
        private readonly ProductContext _dbContext;

        public ProductRepository(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteProduct(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                Save();
            }
        }

        public Product GetProductById(int productId)
        {
            // Fetch the product including the category details
            var product = _dbContext.Products
                 // Fetch the category using ProductCategoryId
                .FirstOrDefault(p => p.ID == productId);
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            // Include the category for each product
            return _dbContext.Products
                 // Fetch the category using ProductCategoryId
                .ToList();
        }

        public void InsertProduct(Product product)
        {
            // Ensure the category exists
            var categoryExists = _dbContext.Categories.Any(c => c.ID == product.ProductCategoryId);
            if (!categoryExists)
            {
                throw new ArgumentException("The specified category does not exist.");
            }

            // Add the product to the context
            _dbContext.Products.Add(product);
            Save();
        }



        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = _dbContext.Products.Find(product.ID);
            if (existingProduct != null)
            {
                // Update the properties you want to change
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.ProductCategoryId = product.ProductCategoryId;

                Save();
            }
        }
        public bool CategoryExists(int categoryId)
        {
            return _dbContext.Categories.Any(c => c.ID == categoryId);
        }

    }
}
