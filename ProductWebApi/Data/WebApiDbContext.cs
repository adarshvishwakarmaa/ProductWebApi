using Microsoft.EntityFrameworkCore;
using ProductWebApi.Models.Domains;

namespace ProductWebApi.Data
{
    public class WebApiDbContext:DbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }
            public DbSet<Product>Products { get; set; }
        
    }
}
