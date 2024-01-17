using Microsoft.EntityFrameworkCore;
using MiniPosInventorySystem.Web.API.Models;

namespace MiniPosInventorySystem.Web.API
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }
        public DbSet<Product> Products
        {
            get;
            set;
        }
        public DbSet<Brand> Brands
        {
            get;
            set;
        }
        public DbSet<Category> Categories
        {
            get;
            set;
        }

        public DbSet<Unit> Units
        {
            get;
            set;
        }

    }
}
