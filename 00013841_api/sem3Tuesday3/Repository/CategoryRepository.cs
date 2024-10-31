using Microsoft.EntityFrameworkCore;
using sem3Tuesday3.DbContexts;
using sem3Tuesday3.Models;

namespace sem3Tuesday3.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductContext _productContext;
        public CategoryRepository(ProductContext productContext)
        {
            _productContext = productContext;
        }
        public void DeleteCategory(int CategoryID)
        {
            var category = _productContext.Categories.Find(CategoryID);
            _productContext.Categories.Remove(category);
            Save();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _productContext.Categories.ToList();
        }

        public Category GetCategoryId(int Id)
        {
            var c = _productContext.Categories.Find(Id);
            return c;
        }

        public void Insertcategory(Category category)
        {
            _productContext.Categories.Add(category);
            Save();
        }

        public void UpdateCategory(Category category)
        {
           _productContext.Entry(category).State = EntityState.Modified;
            Save();
        }
        public void Save()
        {
            _productContext.SaveChanges();
        }
    }
}
