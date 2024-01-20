using GuestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuestAPI.Data
{
    /// <summary>
    /// Represents the application's database context.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet for Guests.
        /// </summary>
        public DbSet<Guest> Guests { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for configuring the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configures the model for the database context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Guest entity
            modelBuilder.Entity<Guest>().HasKey(g => g.Id);
            modelBuilder.Entity<Guest>().Property(g => g.Title).IsRequired();
            modelBuilder.Entity<Guest>().Property(g => g.FirstName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Guest>().Property(g => g.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Guest>().Property(g => g.BirthDate).IsRequired();
            modelBuilder.Entity<Guest>().Property(g => g.Email).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Guest>().HasIndex(g => g.Email).IsUnique();
            modelBuilder.Entity<Guest>().Property(g => g.PhoneNumbers)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

    }
}
