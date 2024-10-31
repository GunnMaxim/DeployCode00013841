namespace sem3Tuesday3.Models
{
    public class CreateProduct
    {

      
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ProductCategoryId { get; set; } // This is the foreign key
    }
}
