using System.Collections.Generic;
using System.Reflection.Emit;
using VictuZ_2._0.Models.Sessions;
using Microsoft.EntityFrameworkCore;

namespace VictuZ_2._0.Data
{
    public class SessionDbContext : DbContext
    {
        public DbSet<Session> Voorbeelden { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VictuZDb;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify the table names
            modelBuilder.Entity<Session>().ToTable("Sessions");

            // Data seeding
            Session voorbeeldEntity = new Session
            {
                Id = 1,
                Title = "Voorbeeld",
                Description = "Dit is een voorbeeld",
                ActivityDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1),
                CreatedById = 1,
                LocationId = 1
            };

            modelBuilder.Entity<Session>().HasData(voorbeeldEntity);
        }
    }
}
