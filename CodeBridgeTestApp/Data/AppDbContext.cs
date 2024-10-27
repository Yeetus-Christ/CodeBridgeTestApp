using CodeBridgeTestApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodeBridgeTestApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        public DbSet<Dog> Dogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dog>().HasData(
                new Dog() { Id = 1, Name = "Neo", Color = "red & amber", TailLength = 22, Weight = 32},
                new Dog() { Id = 2, Name = "Jessy", Color = "black & white", TailLength = 7, Weight = 14 }
            );
        }
    }
}
