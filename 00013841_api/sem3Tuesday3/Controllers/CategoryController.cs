using Microsoft.AspNetCore.Mvc;
using sem3Tuesday3.Models;
using sem3Tuesday3.Repository;
using System.Transactions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace sem3Tuesday3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        // GET: api/Category
        [HttpGet]
        public IActionResult Get()
        {
            var cate = _categoryRepository.GetCategories();
            return new OkObjectResult(cate);
        }


        // GET: api/Category/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetGetByID(int id)
        {
            var cate = _categoryRepository.GetCategoryId(id);
            return new OkObjectResult(cate);
        }

        // POST: api/Category
        [HttpPost]
        public IActionResult Post([FromBody] Category cate)
        {
            using (var scope = new TransactionScope())
            {
                _categoryRepository.Insertcategory(cate);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = cate.ID }, cate);
            }
        }


        // PUT: api/Category/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Category category)
        {
            if (category != null)
            {
                using (var scope = new TransactionScope())
                {
                    _categoryRepository.UpdateCategory(category);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _categoryRepository.DeleteCategory(id);
            return new OkResult();
        }
    }
}
