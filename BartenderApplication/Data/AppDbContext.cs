using BartenderApp.Models;
using Microsoft.EntityFrameworkCore;


namespace BartenderApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Cocktail> Cocktails => Set<Cocktail>();
        public DbSet<Order> Orders => Set<Order>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cocktail>().HasData(
            new Cocktail { Id = 1, Name = "Margarita", Price = 9.50m, Description = "Tequila, lime, triple sec, salted rim" },
            new Cocktail { Id = 2, Name = "Old Fashioned", Price = 11.00m, Description = "Bourbon, bitters, sugar, orange twist" },
            new Cocktail { Id = 3, Name = "Mojito", Price = 10.00m, Description = "Rum, mint, lime, soda" }
            );
        }
    }
}