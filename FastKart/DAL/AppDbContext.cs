using FastKart.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastKart.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Footer> Footers { get; set; }
        public DbSet<Slider> Sliders { get; set; }

    }
}
