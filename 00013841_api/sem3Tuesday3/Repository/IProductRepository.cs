using sem3Tuesday3.Models;

namespace sem3Tuesday3.Repository
{
    public interface IProductRepository
    {
        void InsertProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int productId);
        Product GetProductById(int Id);
        IEnumerable<Product> GetProducts();
        
            // Other method signatures
         bool CategoryExists(int categoryId);
        
    }
}
