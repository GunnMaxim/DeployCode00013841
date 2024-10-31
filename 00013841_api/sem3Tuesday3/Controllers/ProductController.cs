using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sem3Tuesday3.Models;
using sem3Tuesday3.Repository;
using System.Transactions;

namespace sem3Tuesday3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Product
        [HttpGet]
        public IActionResult Get()
        {
            var products = _productRepository.GetProducts(); // Make sure this fetches categories if needed
            return Ok(products);
        }

        // GET: api/Product/5
        [HttpGet("{id}", Name = "GetProducts")]
        public IActionResult GetByID(int id)
        {
            var product = _productRepository.GetProductById(id); // Ensure it includes Category
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST: api/Product
        [HttpPost]
        [HttpPost]
        public IActionResult Post([FromBody] CreateProduct createProductDto)
        {
            if (createProductDto == null)
            {
                return BadRequest();
            }

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                ProductCategoryId = createProductDto.ProductCategoryId // Associate with the existing Category
            };

            _productRepository.InsertProduct(product);

            return CreatedAtAction(nameof(GetByID), new { id = product.ID }, product); // Returns the new product with its generated ID
        }


       
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Product product)
        {
            if (product == null || product.ProductCategoryId == 0)
            {
                return BadRequest("Invalid product data or category ID.");
            }

            if (id != product.ID)
            {
                return BadRequest("Product ID mismatch.");
            }

            using (var scope = new TransactionScope())
            {
                _productRepository.UpdateProduct(product);
                scope.Complete();
                return Ok();
            }
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productRepository.DeleteProduct(id);
            return Ok();
        }
    }
}
