using AuthTest.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthTest.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshTokens");
            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
