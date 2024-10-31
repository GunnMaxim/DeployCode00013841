using sem3Tuesday3.Models;

namespace sem3Tuesday3.Repository
{
    public interface ICategoryRepository
    {
        void Insertcategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int CategoryID);
        Category GetCategoryId(int Id);
        IEnumerable<Category> GetCategories();
    }
}
