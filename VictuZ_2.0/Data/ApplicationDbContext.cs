using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VictuZ_2._0.Models.Feedbacks;
using VictuZ_2._0.Models.Locations;
using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Suggestions;
using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        // DbSet properties voor alle entiteiten, zonder Users omdat IdentityDbContext dit al bevat
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionRegistration> SessionRegistrations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Suggestion> Suggestions { get; set; }
        public DbSet<SuggestionLike> SuggestionLikes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Verwijder OnConfiguring als je de connectiestring in Program.cs instelt.
        // Als je het toch wilt behouden, zorg ervoor dat het alleen wordt geconfigureerd als het nog niet is gedaan.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Overweeg om de connectiestring naar appsettings.json te verplaatsen voor betere configuratie
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VictuZDb;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Verwijder de discriminator configuratie aangezien we geen overerving meer gebruiken
            // Geen .HasDiscriminator() meer

            // Configureer relaties en constraints

            // Session en User (CreatedBy)
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
                .HasOne(sr => sr.Session)
                .WithMany(s => s.ActivityRegistrations)
                .HasForeignKey(sr => sr.SessionId);

            modelBuilder.Entity<SessionRegistration>()
                .HasOne(sr => sr.User)
                .WithMany(u => u.ActivityRegistrations)
                .HasForeignKey(sr => sr.UserId);

            // Feedback relaties
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
                .HasOne(s => s.User)
                .WithMany(u => u.Suggestions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.SetNull); // Omdat UserId nullable is

            // SuggestionLike relaties
            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.Suggestion)
                .WithMany(s => s.SuggestionLikes)
                .HasForeignKey(sl => sl.SuggestionId);

            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.User)
                .WithMany()
                .HasForeignKey(sl => sl.UserId);

            // Configureer IdentityRole<int> tabel
            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable("AspNetRoles");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
            });

            // Laat IdentityDbContext de configuratie van IdentityUserRole<int> beheren
            // Verwijder expliciete configuratie om conflicten te voorkomen
        }
    }
}
