using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Core.Models.Feedbacks;
using Core.Models.Locations;
using Core.Models.Newss;
using Core.Models.Sessions;
using Core.Models.Suggestions;
using Core.Models.Users;

namespace Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        // DbSet properties voor alle entiteiten
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionRegistration> SessionRegistrations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<SuggestionLike> SuggestionLikes { get; set; }
        public DbSet<News> News { get; set; }

        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Als je de connectiestring in Program.cs instelt, is OnConfiguring niet nodig
        // Maar als je het toch wilt behouden:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Het is beter om de connectiestring in appsettings.json te plaatsen
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VictuZDb;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        // Modelconfiguratie
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configureer relaties en constraints

            // User en Session (CreatedBy)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.CreatedBy)
                .WithMany(u => u.CreatedActivities)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            // Session en Location
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Location)
                .WithMany(l => l.ScheduledActivities)
                .HasForeignKey(s => s.LocationId)
                .OnDelete(DeleteBehavior.Restrict);

            // SessionRegistration relaties
            modelBuilder.Entity<SessionRegistration>()
                .HasKey(sr => sr.Id); // Primair sleutel

            modelBuilder.Entity<SessionRegistration>()
                .HasOne(sr => sr.Session)
                .WithMany(s => s.SessionRegistrations)
                .HasForeignKey(sr => sr.SessionId);

            modelBuilder.Entity<SessionRegistration>()
                .HasOne(sr => sr.User)
                .WithMany(u => u.SessionRegistrations)
                .HasForeignKey(sr => sr.UserId);

            // Feedback relaties
            modelBuilder.Entity<Feedback>()
                .HasKey(f => f.Id); // Primair sleutel

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Session)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SessionId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(f => f.UserId);

            // Suggestion relaties
            modelBuilder.Entity<Suggestion>()
                .HasKey(s => s.Id); // Primair sleutel

            modelBuilder.Entity<Suggestion>()
                .HasOne(s => s.CreatedBy)
                .WithMany(u => u.Suggestions)
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            // SuggestionLike relaties
            modelBuilder.Entity<SuggestionLike>()
                .HasKey(sl => sl.Id); // Primair sleutel

            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.Suggestion)
                .WithMany(s => s.SuggestionLikes)
                .HasForeignKey(sl => sl.SuggestionId);

            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.User)
                .WithMany()
                .HasForeignKey(sl => sl.UserId);
        }
    }
}
