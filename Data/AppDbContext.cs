using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inquiry> Inquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Trousers", Slug = "trousers", ImageUrl = "" },
                new Category { Id = 2, Name = "Hoodies", Slug = "hoodies", ImageUrl = "" },
                new Category { Id = 3, Name = "Shorts", Slug = "shorts", ImageUrl = "" },
                new Category { Id = 4, Name = "Sports Wear", Slug = "sports-wear", ImageUrl = "" },
                new Category { Id = 5, Name = "Leather Jackets", Slug = "leather-jackets", ImageUrl = "" },
                new Category { Id = 6, Name = "Track Suits", Slug = "track-suits", ImageUrl = "" },
                new Category { Id = 7, Name = "Suits", Slug = "suits", ImageUrl = "" }
            );
        }
    }
}