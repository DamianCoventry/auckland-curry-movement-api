using acm_rest_api.Models;
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

        public DbSet<PastDinner>? PastDinner { get; set; }

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

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Organiser)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.OrganiserID)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .HasOne(x => x.RotY)
                .WithMany(x => x.Notifications)
                .HasForeignKey(x => x.RotYYear);

            modelBuilder.Entity<Exemption>()
                .HasOne(x => x.FoundingFather)
                .WithMany(x => x.ExemptionsGiven)
                .HasForeignKey(x => x.FoundingFatherID);

            modelBuilder.Entity<Exemption>()
                .HasOne(x => x.Member)
                .WithMany(x => x.ExemptionsReceived)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Violation>()
                .HasOne(x => x.FoundingFather)
                .WithMany(x => x.ViolationsGiven)
                .HasForeignKey(x => x.FoundingFatherID);

            modelBuilder.Entity<Violation>()
                .HasOne(x => x.Member)
                .WithMany(x => x.ViolationsReceived)
                .HasForeignKey(x => x.MemberID);
        }
    }
}
