using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}


// EF Core automatically calls OnModelCreating() method when building database models, generating migrations and creating tables. Simple analogy “Before creating database tables, tell me all your rules/configurations.”
//	We used override because DbContext already contains OnModelCreating() and we are customizing or extending it again.
// ModelBuilder is a helper object used by EF Core to configure all database tables and entity rules during application startup. 
//modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
//	Above line simply means find all Fluent API configuration classes in this project and apply them automatically.
