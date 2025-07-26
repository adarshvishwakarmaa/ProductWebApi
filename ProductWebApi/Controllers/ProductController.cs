using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Mono.TextTemplating;
using Newtonsoft.Json;
using ProductWebApi.global;
using ProductWebApi.helper;
using ProductWebApi.Models.Domains;
using System.Net.Http.Json;

namespace ProductWebApi.Controllers
{
    public class ProductController : Controller
    {
        static Helper api = new Helper();
        Global global = new Global();
        private readonly IConfiguration configuration;
        public ProductController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                using (HttpClient client = api.Intial())
                {
                    using (HttpResponseMessage resp = await client.GetAsync("api/ProductApi"))
                    {
                        if (resp.IsSuccessStatusCode)
                        {
                            var result = resp.Content.ReadAsStringAsync().Result;
                            var mydata = JsonConvert.DeserializeObject<List<Product>>(result);
                            return View(mydata.ToList());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;    
            }
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                using (HttpClient client = api.Intial()) { 
                    using(HttpResponseMessage resp = await client.PostAsJsonAsync("api/ProductApi", product))
                    {
                        if (resp.IsSuccessStatusCode) {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch (Exception ex) { 
                string message = ex.Message;
            }
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Product product = new Product();
            try
            {
                using(HttpClient client = api.Intial())
                {
                    using(HttpResponseMessage res = await client.GetAsync($"api/ProductApi/{id}"))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            var result = res.Content.ReadAsStringAsync().Result;
                             product = JsonConvert.DeserializeObject<Product>(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = new();
            try
            {
                using (HttpClient client =  api.Intial())
                {
                    using (HttpResponseMessage res = await client.GetAsync($"api/ProductApi/{id}"))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                            var result = res.Content.ReadAsStringAsync().Result;
                            product = JsonConvert.DeserializeObject<Product>(result);
                            
                        }
                    }
                }
            }
            catch (Exception ex) {
                string message = ex.Message;
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(Product product)
        {
            try {
                using (HttpClient client = api.Intial())
                {
                    using (HttpResponseMessage res = await client.PutAsJsonAsync($"api/ProductApi/{product.ProductId}", product))
                    {
                        if (res.IsSuccessStatusCode)
                        {
                             return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                string message = ex.Message;
            }
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult>Delete(int id)
        {
            try
            {
                using(HttpClient client = api.Intial())
                {
                    using(HttpResponseMessage res = await client.DeleteAsync($"api/ProductApi/{id}")){
                        if (res.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult>GetData(string alldata)
        {
            var alldataArr = alldata.Split("^");
            int pageIndex = Convert.ToInt32(alldataArr[0]);
            int pageSize = Convert.ToInt32(alldataArr[1]);
            string Name = alldataArr[2];
            string Description = alldataArr[3];
            string Price = alldataArr[4];
            string Quantity = alldataArr[5];
            string Brand = alldataArr[6];
            string Category = alldataArr[7];
            string CreatedAt = alldataArr[8];
            string DiscountPercentage = alldataArr[9];
            string IsActive = alldataArr[10];
            string SupplierName = alldataArr[11];




            List<Product> filterProductData = await GetAllData();
         
            if (!string.IsNullOrEmpty(Name))
            {
                filterProductData = filterProductData.Where(s => s.Name.Contains(Name, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(Description))
            {
                filterProductData = filterProductData.Where(s => s.Description.Contains(Description, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(Price))
            {
                filterProductData = filterProductData.Where(s => s.Price.ToString().Contains(Price, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(Quantity))
            {
                filterProductData = filterProductData.Where(s => s.Quantity.ToString().Contains(Quantity, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(Brand))
            {
                filterProductData = filterProductData.Where(s => s.Brand.Contains(Brand, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(Category))
            {
                filterProductData = filterProductData.Where(s => s.Category.Contains(Category, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(CreatedAt))
            {
                filterProductData = filterProductData.Where(s => s.CreatedAt.ToString("yyyy-MM-dd").Contains(CreatedAt, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(DiscountPercentage))
            {
                filterProductData = filterProductData.Where(s => s.DiscountPercentage.ToString().Contains(DiscountPercentage, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (!string.IsNullOrEmpty(IsActive))
            {
                filterProductData = filterProductData.Where(s => s.IsActive.ToString().Contains(IsActive, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            if (!string.IsNullOrEmpty(SupplierName))
            {
                filterProductData = filterProductData.Where(s => s.SupplierName.Contains(SupplierName, StringComparison.OrdinalIgnoreCase)).ToList();
            }



         
            #region Pagination
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int allRecords = filterProductData.Count();
            int startPage = pageIndex - 5;
            int endPage = pageIndex + 4;
            int totalPage = (int)Math.Ceiling((decimal)allRecords / (decimal)pageSize);
            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;

            }
            if (endPage > totalPage)
            {
                endPage = totalPage;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            #endregion
            ViewBag.AllRecord = allRecords;
            ViewBag.TotalPage = totalPage;
            ViewBag.CurrentPage = pageIndex;
            ViewBag.EndPage = endPage;
            ViewBag.StartPage = startPage;
            int excludedRecord = (pageIndex - 1) * pageSize;
            filterProductData = filterProductData.Skip(excludedRecord).Take(pageSize).ToList();
            ViewBag.ShowRecord = filterProductData.Count();
            ViewBag.getAllData = filterProductData.ToList();
            return PartialView("_ProductPagination");
        }
        [HttpGet]
        public async Task<List<Product>> GetAllData()
        {
            List<Product> list = new();
            try
            {
                using (HttpClient client = api.Intial())
                {
                    using (HttpResponseMessage resp = await client.GetAsync("api/ProductApi"))
                    {
                        if (resp.IsSuccessStatusCode)
                        {
                            var result = resp.Content.ReadAsStringAsync().Result;
                            list = JsonConvert.DeserializeObject<List<Product>>(result);
                            return list;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return null;
        }

    }
}
