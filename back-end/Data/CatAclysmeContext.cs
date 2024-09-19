using Microsoft.EntityFrameworkCore;
using CatAclysmeApp.Models;

namespace CatAclysmeApp.Data
{
    public class CatAclysmeContext : DbContext
    {
        public CatAclysmeContext(DbContextOptions<CatAclysmeContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<PlayerHand> PlayerHands { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Build> Builds { get; set; }
        public DbSet<GameCard> GameCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Spécifier le nom exact de la table
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Card>().ToTable("Card");
            modelBuilder.Entity<Deck>().ToTable("Deck");
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Score>().ToTable("Score");
            modelBuilder.Entity<PlayerHand>().ToTable("PlayerHand");
            modelBuilder.Entity<Turn>().ToTable("Turn");
            modelBuilder.Entity<Build>().ToTable("Build");
            modelBuilder.Entity<GameCard>().ToTable("GameCard");

            // Configuration des relations spécifiques (par exemple, clé composite)
            modelBuilder.Entity<Build>()
                .HasKey(b => new { b.DeckId, b.CardId }); // Clé composite pour Build

            modelBuilder.Entity<GameCard>()
                .HasKey(gc => new { gc.PlayerId, gc.CardId, gc.GameId }); // Clé composite pour GameCard
        }
    }
}
