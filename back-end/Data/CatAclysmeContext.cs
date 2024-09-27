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
            // Configuration des autres entités
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<Card>().ToTable("Card");
            modelBuilder.Entity<Deck>().ToTable("Deck");
            modelBuilder.Entity<Game>().ToTable("Game");
            modelBuilder.Entity<Score>().ToTable("Score");
            modelBuilder.Entity<PlayerHand>().ToTable("PlayerHand");
            modelBuilder.Entity<Turn>().ToTable("Turn");
            modelBuilder.Entity<Build>().ToTable("Build");
            modelBuilder.Entity<GameCard>().ToTable("GameCard");

            modelBuilder.Entity<PlayerHand>()
                .HasKey(ph => new {ph.GameId, ph.CardId, ph.PlayerId});

            // Clé composite pour Build
            modelBuilder.Entity<Build>()
                .HasKey(b => new { b.DeckId, b.CardId });

            // Clé composite pour GameCard
            modelBuilder.Entity<GameCard>()
                .HasKey(gc => new { gc.PlayerId, gc.CardId, gc.GameId });

            // Configuration explicite des relations entre Game et Player
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Player)
                .WithMany()
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.Player_1)
                .WithMany()
                .HasForeignKey(g => g.PlayerId_1)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration explicite des relations entre Score et Player
            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player)
                .WithMany()
                .HasForeignKey(s => s.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Score>()
                .HasOne(s => s.Player_1)
                .WithMany()
                .HasForeignKey(s => s.PlayerId_1)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration explicite des relations pour Turn et Card
            modelBuilder.Entity<Turn>()
                .HasOne(t => t.Card)
                .WithMany()
                .HasForeignKey(t => t.CardId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Turn>()
                .HasOne(t => t.Card_1)
                .WithMany()
                .HasForeignKey(t => t.CardId_1)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuration explicite des relations entre Player et Deck
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Deck)
                .WithOne(d => d.Player)
                .HasForeignKey<Deck>(d => d.PlayerId)
                .IsRequired();   
        }
    }
}
