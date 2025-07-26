# 🛍️ Product Management Web App (ASP.NET Core MVC + Web API)

This is a full-stack Product Management system built with **ASP.NET Core MVC** as the frontend client and **ASP.NET Core Web API** as the backend service. It allows you to perform all CRUD operations, with client-side filtering, pagination, and modern Bootstrap design.

---

## 📦 Technologies Used

| Tech              | Role                                      |
|------------------|-------------------------------------------|
| ASP.NET Core MVC | Frontend web application (UI)             |
| ASP.NET Core Web API | RESTful backend service                |
| Entity Framework | ORM for API database operations           |
| HttpClient       | API consumption in MVC                    |
| Newtonsoft.Json  | JSON serialization/deserialization        |
| jQuery + AJAX    | Frontend interactivity & dynamic reloads  |
| Bootstrap        | Styling & responsive layout               |


🖼️ Screenshots
<img width="1810" height="899" alt="Screenshot 2025-07-26 213228" src="https://github.com/user-attachments/assets/fddec14f-7bfb-43a0-aaae-cde6ca0acc67" />

<img width="1802" height="886" alt="Screenshot 2025-07-26 213150" src="https://github.com/user-attachments/assets/8c89b657-bd76-421d-9239-59da23d3cdf6" />


---
📡 API Communication (Helper Class)

public class Helper
{
    public async Task<HttpResponseMessage> CallApi(string url, HttpMethod method, object data = null)
    {
        using var client = new HttpClient();
        if (method == HttpMethod.Post || method == HttpMethod.Put)
        {
            var content = JsonConvert.SerializeObject(data);
            return await client.SendAsync(new HttpRequestMessage(method, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });
        }
        return await client.SendAsync(new HttpRequestMessage(method, url));
    }
}



🎮 Controller Snippet (ProductController.cs)

public class ProductController : Controller
{
    static Helper api = new Helper();
    Global global = new Global();
    private readonly IConfiguration configuration;

    public ProductController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IActionResult> Index(int page = 1, int pageSize = 5, string search = "")
    {
        string apiUrl = $"{global.BaseUrl}product?page={page}&pageSize={pageSize}&search={search}";
        var response = await api.CallApi(apiUrl, HttpMethod.Get);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<Product>>(json);
            return View(products);
        }
        return View(new List<Product>());
    }

    // Other actions: Create, Edit, Details, Delete...
}


## 🧱 Project Architecture Overview

```yaml
ProductWebApp/
│
├── Controllers/
│   └── ProductController.cs         # Handles UI logic, consumes Web API
│
├── Models/
│   └── Domains/
│       └── Product.cs               # Product entity used by both MVC & API
│
├── Views/
│   └── Product/
│       ├── Index.cshtml             # Product list with pagination and filter
│       ├── Create.cshtml            # Add product form
│       ├── Edit.cshtml              # Edit product form
│       ├── Details.cshtml           # View product details
│       ├── Delete.cshtml            # Confirm delete
│       └── _ProductRowPartial.cshtml # Partial View for product rows
│
├── wwwroot/
│   └── js / css / lib (Bootstrap, jQuery)
│
├── global/
│   └── Global.cs                    # API base URL
│
├── helper/
│   └── Helper.cs                    # API calling logic
│
├── appsettings.json                # API URL configuration
└── Startup.cs / Program.cs         # App bootstrapping



