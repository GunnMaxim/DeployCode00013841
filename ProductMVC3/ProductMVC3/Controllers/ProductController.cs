using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductMVC3.Models;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;

namespace ProductMVC3.Controllers
{
    [ApiController]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly string BaseUrl = "http://localhost:5180/";
        Uri baseAddress = new Uri("http://localhost:5180/");
        private readonly HttpClient _httpClient;


        // GET: Product
        [HttpGet("")]
        public async Task<ActionResult> Index()
        {
            List<Product> ProdInfo = new List<Product>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/Product");

                if (Res.IsSuccessStatusCode)
                {
                    var PrResponse = await Res.Content.ReadAsStringAsync();
                    ProdInfo = JsonConvert.DeserializeObject<List<Product>>(PrResponse);
                }
                return View(ProdInfo);
            }
        }

        // GET: Product/Details/5
        // ProductController.cs
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            Product product = null;

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
                        product.Category = JsonConvert.DeserializeObject<Category>(categoryData); // Add category to product
                    }
                }

                if (product == null)
                {
                    return NotFound(); // Return 404 if the product is not found
                }
            }

            return View(product); // Pass product to the view, which now includes category information
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            // Optionally, you can populate categories here
            return View();
        }

        // POST: Product/Create
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonContent = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Product", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(product);
        }



        /*
        // ProductController.cs
        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            // Fetch the list of categories for the dropdown
            List<Category> categories = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Category"); // Fetch categories
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(data);
                }
            }

            ViewBag.CategoryList = new SelectList(categories, "ID", "Name"); // Populate the dropdown list
            return View(); // Return the view without any product data
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [HttpPost]

        public async Task<IActionResult> Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Create an object with only the necessary properties
                    var productToCreate = new
                    {
                        Id = newProduct.ID,
                        Name = newProduct.Name,
                        Description = newProduct.Description,
                        Price = newProduct.Price,
                        ProductCategoryId = newProduct.ProductCategoryId
                    };

                    // Serialize the new product to JSON
                    var jsonContent = JsonConvert.SerializeObject(productToCreate);
                    var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // Explicitly set content type

                    // Send the POST request
                    HttpResponseMessage response = await client.PostAsync("api/Product", contentString);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index)); // Redirect if successful
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while creating the product.");
                    }
                }
            }

            await PopulateCategories();
            return View(newProduct);
        }*/







        // GET: Product/Edit/5




        [HttpGet("Edit/{id}")]
    public async Task<ActionResult> Edit(int id)
    {
        Product product = null;

        // Fetch the product details
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync($"api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<Product>(data);
            }
        }

        // Fetch the list of categories
        List<Category> categories = new List<Category>();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("api/Category");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                categories = JsonConvert.DeserializeObject<List<Category>>(data);
            }
        }

        // Populate the dropdown list
        ViewBag.CategoryList = new SelectList(categories, "ID", "Name");

        return View(product);
    }




    [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Product updatedProduct)
        {
            // Ensure that ProductCategoryId is set and valid
            if (updatedProduct.ProductCategoryId <= 0)
            {
                ModelState.AddModelError("ProductCategoryId", "Please select a valid category.");
                await PopulateCategories(); // Repopulate the categories in case of error
                return View(updatedProduct); // Return the view if validation fails
            }

            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Serialize the updated product with JSON content
                    var jsonContent = JsonConvert.SerializeObject(updatedProduct);
                    var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json"); // explicitly set content-type

                    // Send the PUT request
                    HttpResponseMessage response = await client.PutAsync($"api/Product/{id}", contentString);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Return the view with the current data if update fails
                        ModelState.AddModelError(string.Empty, "An error occurred while updating the product.");
                    }
                }
            }

            // Repopulate categories if validation fails
            await PopulateCategories();
            return View(updatedProduct); // Return the view with validation errors
        }

        private async Task PopulateCategories()
        {
            List<Category> categories = new List<Category>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Category"); // Fetch categories
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Category>>(data);
                }
            }

            ViewBag.CategoryList = new SelectList(categories, "ID", "Name"); // Populate the dropdown list
        }





        // GET: Product/Delete/5
        [HttpGet("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            Product product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(data);
                }
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost("Delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.DeleteAsync($"api/Product/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }
    }
}
