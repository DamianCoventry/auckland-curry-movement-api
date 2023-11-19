using auckland_curry_movement_api.Models;
using Microsoft.EntityFrameworkCore;

namespace auckland_curry_movement_api
{
    public class AcmDatabaseContext : DbContext
    {
        public AcmDatabaseContext(DbContextOptions<AcmDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant>? Restaurant { get; set; }

        public DbSet<Attendee>? Attendee { get; set; }

        public DbSet<Club>? Club { get; set; }

        public DbSet<Dinner>? Dinner { get; set; }

        public DbSet<Exemption>? Exemption { get; set; }

        public DbSet<KotC>? KotC { get; set; }

        public DbSet<Level>? Level { get; set; }

        public DbSet<Member>? Member { get; set; }

        public DbSet<Notification>? Notification { get; set; }

        public DbSet<Reservation>? Reservation { get; set; }

        public DbSet<Violation>? Violation { get; set; }

        public DbSet<RotY>? RotY { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RotY>()
                .HasKey("Year");

            modelBuilder.Entity<Club>()
                .HasMany(x => x.Members)
                .WithMany(x => x.Clubs)
                .UsingEntity<MemberClub>();

            modelBuilder.Entity<Member>()
                .HasMany(x => x.Dinners)
                .WithMany(x => x.Members)
                .UsingEntity<Attendee>();

            modelBuilder.Entity<Member>()
                .HasOne(x => x.CurrentLevel)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.CurrentLevelID)
                .IsRequired();
        }
    }
}
