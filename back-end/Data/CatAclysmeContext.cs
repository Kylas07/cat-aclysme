using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;

namespace CatAclysmeApp.Data
{
    public class CatAclysmeContext : DbContext
    {
        public CatAclysmeContext(DbContextOptions<CatAclysmeContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Spécifier le nom exact de la table
            modelBuilder.Entity<Player>().ToTable("Player");
        }
    }
}