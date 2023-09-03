using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ProductMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";

        public ProductController() //contructor khoi tao cac gia tri
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "http://localhost:5039/api/ProductsControllers";
        }

        public async Task<IActionResult> Index() //trang chu, return list product
        {
            HttpResponseMessage response1 = await client.GetAsync(ProductApiUrl); //get response called from api
            string strData1 = await response1.Content.ReadAsStringAsync(); //tostring
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Product> listProduct = JsonSerializer.Deserialize<List<Product>>(strData1, options1); //gan vao object
            return View(listProduct); //tra ve view 1 list product va cate
        }

        public async Task<IActionResult> Details(int id) //get product by id
        {
            ProductApiUrl = $"http://localhost:5039/api/ProductsControllers/{id}"; // get product by id api
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            return View(product);
        }

        public async Task<IActionResult> Create() //Mo view create form
        {
            HttpResponseMessage response2 = await client.GetAsync("http://localhost:5039/api/Category"); //get response called from api
            string strData2 = await response2.Content.ReadAsStringAsync(); //tostring
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(strData2, options2);
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product p) //nhan gia tri cua product -> create
        {
            p.ProductId = 0; //auto increament
            var jsonVal = JsonSerializer.Serialize(p);
            HttpContent c = new StringContent(jsonVal, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(ProductApiUrl, c); //post method
            string strData = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index", "Product"); //ve trang chu
        }

        public async Task<ActionResult> EditAsync(int id) //mo view edit truyen vao gia tri product get theo id
        {
            HttpResponseMessage response2 = await client.GetAsync("http://localhost:5039/api/Category"); //get response called from api
            string strData2 = await response2.Content.ReadAsStringAsync(); //tostring
            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Category> categories = JsonSerializer.Deserialize<List<Category>>(strData2, options2);

            ProductApiUrl = $"http://localhost:5039/api/ProductsControllers/{id}";
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);

            ProductCategoryList productCategoryList = new ProductCategoryList()
            {
                CustomObject1 = product,
                CustomObject2 = categories
            };
            return View(productCategoryList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection collection)
        {
            Product p = new Product();
            p.ProductId = Int32.Parse(collection["ProductId"]);
            p.ProductName = collection["ProductName"];
            p.CategoryId = Int32.Parse(collection["CategoryId"]);
            p.UnitsInStock = Int32.Parse(collection["UnitsInStock"]);
            p.UnitPrice = Decimal.Parse(collection["UnitPrice"]);
            var jsonVal = JsonSerializer.Serialize(p);
            HttpContent c = new StringContent(jsonVal, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(ProductApiUrl, c);
            string strData = await response.Content.ReadAsStringAsync();
            ProductApiUrl = $"http://localhost:5039/api/ProductsControllers/{p.ProductId}";
            response = await client.GetAsync(ProductApiUrl);
            strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Product product = JsonSerializer.Deserialize<Product>(strData, options);
            return RedirectToAction("Index", "Product"); //xoa xong ve trang chu
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            ProductApiUrl = $"http://localhost:5039/api/ProductsControllers/{id}";
            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index", "Product"); //xoa xong ve trang chu
        }
    }

    public class ProductCategoryList
    {
        public Product CustomObject1 { get; set; }
        public List<Category> CustomObject2 { get; set; }
    }
}
