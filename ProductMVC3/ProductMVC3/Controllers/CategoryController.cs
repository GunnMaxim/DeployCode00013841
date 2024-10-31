using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMVC3.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ProductMVC3.Controllers
{
    [ApiController]
    [Route("Category")]
    public class CategoryController : Controller
    {
        private readonly string BaseUrl = "http://localhost:5180/";

        // GET: Category
        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            List<Category> ProdInfo = new List<Category>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Category");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = await Res.Content.ReadAsStringAsync();
                    ProdInfo = JsonConvert.DeserializeObject<List<Category>>(PrResponse);
                }
                return View(ProdInfo);
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Product product = null;
            Category category = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Fetch product details
                HttpResponseMessage productResponse = await client.GetAsync($"api/Product/{id}");
                if (productResponse.IsSuccessStatusCode)
                {
                    var productData = await productResponse.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(productData);

                    // Fetch category details using ProductCategoryId
                    HttpResponseMessage categoryResponse = await client.GetAsync($"api/Category/{product.ProductCategoryId}");
                    if (categoryResponse.IsSuccessStatusCode)
                    {
                        var categoryData = await categoryResponse.Content.ReadAsStringAsync();
                        category = JsonConvert.DeserializeObject<Category>(categoryData);
                    }
                }

                if (product == null)
                {
                    return NotFound(); // Return 404 if the product is not found
                }
            }

            // Pass both models to the view without a view model
            ViewBag.Product = product;
            ViewBag.Category = category;

            return View(); // Just return the view; access models using ViewBag
        }

        // GET: Category/Create
        [HttpGet("Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Category newCategory)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = JsonConvert.SerializeObject(newCategory);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Category", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(newCategory);
        }

        // GET: Category/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            Category Category = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Category = JsonConvert.DeserializeObject<Category>(data);
                }
            }
            return View(Category);
        }

        // POST: Category/Edit/5
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Category updatedCategory)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = JsonConvert.SerializeObject(updatedCategory);
                var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"api/Category/{id}", contentString);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(updatedCategory);
        }

        // GET: Category/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Category Category = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    Category = JsonConvert.DeserializeObject<Category>(data);
                }
            }
            return View(Category);
        }

        // POST: Category/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"api/Category/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
    }
}
