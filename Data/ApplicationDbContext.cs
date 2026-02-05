using Microsoft.EntityFrameworkCore;
using Vidly.Models;

namespace Vidly.Data  // Adjust if your project uses a different namespace (check Program.cs)
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSets here as you create entities, e.g.:
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }

        // This helps with design-time tools (migrations) if no config in appsettings.json
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseSqlite("Data Source=vidly.db");
        //     }
        // }
    }
}