using auckland_curry_movement_api.Models;
using Microsoft.EntityFrameworkCore;

namespace auckland_curry_movement_api.DatabaseContext
{
    public class AcmDatabaseContext : DbContext
    {
        public AcmDatabaseContext(DbContextOptions<AcmDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Restaurant> Restaurants { get; set; } = null!;
    }
}
