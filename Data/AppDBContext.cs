using HouseholdOnlineStore.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdOnlineStore.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Lab6> Lab6s { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>()
                .HasMany(x => x.Orders)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
		}
	}
}
