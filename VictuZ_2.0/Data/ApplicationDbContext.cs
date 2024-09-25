using Microsoft.EntityFrameworkCore;
using VictuZ_2._0.Models.Users;
using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Feedbacks;
using VictuZ_2._0.Models.Locations;
using VictuZ_2._0.Models.Suggestions;
using VictuZ_2._0.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace VictuZ_2._0.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        // DbSet properties voor alle entiteiten
        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<BoardMember> BoardMembers { get; set; }
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=VictuZDb;Integrated Security=True;TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuratie voor overerving
            modelBuilder.Entity<User>()
                .HasDiscriminator<RoleType>("Role")
                .HasValue<Member>(RoleType.Member)
                .HasValue<BoardMember>(RoleType.BoardMember);

            // Configuratie van relaties en constraints

            // Session en BoardMember (CreatedBy)
            modelBuilder.Entity<Session>()
                .HasOne(s => s.CreatedBy)
                .WithMany(b => b.CreatedActivities)
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
                .HasOne(sr => sr.Member)
                .WithMany(m => m.ActivityRegistrations)
                .HasForeignKey(sr => sr.MemberId);

            // Feedback relaties
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Session)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SessionId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Member)
                .WithMany(m => m.Feedbacks)
                .HasForeignKey(f => f.MemberId);

            // Suggestion relaties
            modelBuilder.Entity<Suggestion>()
                .HasOne(s => s.Member)
                .WithMany(m => m.Suggestions)
                .HasForeignKey(s => s.MemberId)
                .OnDelete(DeleteBehavior.SetNull); // Omdat MemberId nullable is

            // SuggestionLike relaties
            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.Suggestion)
                .WithMany(s => s.SuggestionLikes)
                .HasForeignKey(sl => sl.SuggestionId);

            modelBuilder.Entity<SuggestionLike>()
                .HasOne(sl => sl.Member)
                .WithMany()
                .HasForeignKey(sl => sl.MemberId);

            // Unieke constraints, indices en overige configuraties kunnen hier worden toegevoegd

            base.OnModelCreating(modelBuilder);
        }
    }
}
