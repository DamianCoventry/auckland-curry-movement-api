using acm_models;
using Microsoft.EntityFrameworkCore;

namespace acm_rest_api
{
    public class AcmDatabaseContext : DbContext
    {
        public AcmDatabaseContext(DbContextOptions<AcmDatabaseContext> options)
            : base(options) {}

        public DbSet<Restaurant>? Restaurant { get; set; }

        public DbSet<Attendee>? Attendee { get; set; }

        public DbSet<Club>? Club { get; set; }

        public DbSet<Dinner>? Dinner { get; set; }

        public DbSet<PastDinner>? PastDinner { get; set; }

        public DbSet<Exemption>? Exemption { get; set; }

        public DbSet<KotC>? KotC { get; set; }

        public DbSet<Level>? Level { get; set; }

        public DbSet<Meal>? Meal { get; set; }

        public DbSet<Member>? Member { get; set; }

        public DbSet<MemberStats>? MemberStats { get; set; }

        public DbSet<Notification>? Notification { get; set; }

        public DbSet<Reservation>? Reservation { get; set; }

        public DbSet<Violation>? Violation { get; set; }

        public DbSet<RotY>? RotY { get; set; }

        public DbSet<Membership>? Membership { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RotY>()
                .HasKey("Year");

            modelBuilder.Entity<Membership>()
                .HasOne(x => x.Member);

            modelBuilder.Entity<Meal>()
                .HasKey("ReservationID");

            modelBuilder.Entity<Club>()
                .HasMany(x => x.Memberships)
                .WithMany(x => x.Clubs)
                .UsingEntity<Membership>();

            modelBuilder.Entity<Membership>()
                .HasMany(x => x.Dinners)
                .WithMany(x => x.Memberships)
                .UsingEntity<Attendee>();

            modelBuilder.Entity<Membership>()
                .HasOne(x => x.Level)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.LevelID)
                .IsRequired();

            modelBuilder.Entity<Membership>()
                .HasMany(x => x.Inductees)
                .WithOne(x => x.Sponsor)
                .HasForeignKey(x => x.SponsorID)
                .IsRequired(false);

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
                .HasOne(x => x.Membership)
                .WithMany(x => x.ExemptionsReceived)
                .HasForeignKey(x => x.MemberID);

            modelBuilder.Entity<Violation>()
                .HasOne(x => x.FoundingFather)
                .WithMany(x => x.ViolationsGiven)
                .HasForeignKey(x => x.FoundingFatherID);

            modelBuilder.Entity<Violation>()
                .HasOne(x => x.Membership)
                .WithMany(x => x.ViolationsReceived)
                .HasForeignKey(x => x.MemberID);
        }
    }
}
