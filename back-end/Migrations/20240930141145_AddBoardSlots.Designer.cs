﻿// <auto-generated />
using System;
using CatAclysmeApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace back_end.Migrations
{
    [DbContext(typeof(CatAclysmeContext))]
    [Migration("20240930141145_AddBoardSlots")]
    partial class AddBoardSlots
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CatAclysmeApp.Models.BoardSlot", b =>
                {
                    b.Property<int>("BoardSlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BoardSlotId"));

                    b.Property<int?>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.HasKey("BoardSlotId");

                    b.HasIndex("CardId");

                    b.HasIndex("GameId");

                    b.ToTable("BoardSlot");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Build", b =>
                {
                    b.Property<int>("DeckId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.HasKey("DeckId", "CardId");

                    b.HasIndex("CardId");

                    b.ToTable("Build", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardId"));

                    b.Property<int>("Attack")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardId");

                    b.ToTable("Card", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Deck", b =>
                {
                    b.Property<int>("DeckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeckId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("DeckId");

                    b.HasIndex("PlayerId")
                        .IsUnique();

                    b.ToTable("Deck", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GameId"));

                    b.Property<int>("GameStatus")
                        .HasColumnType("int");

                    b.Property<int>("Player1HP")
                        .HasColumnType("int");

                    b.Property<int>("Player2HP")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId_1")
                        .HasColumnType("int");

                    b.Property<int>("PlayerTurn")
                        .HasColumnType("int");

                    b.Property<int>("TurnCount")
                        .HasColumnType("int");

                    b.HasKey("GameId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerId_1");

                    b.ToTable("Game", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.GameCard", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("CardPosition")
                        .HasColumnType("int");

                    b.Property<int>("CurrentHealth")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.HasKey("PlayerId", "CardId", "GameId");

                    b.HasIndex("CardId");

                    b.HasIndex("GameId");

                    b.ToTable("GameCard", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Player", b =>
                {
                    b.Property<int>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PlayerId");

                    b.ToTable("Player", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Score", b =>
                {
                    b.Property<int>("ScoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScoreId"));

                    b.Property<int>("Player1Score")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId_1")
                        .HasColumnType("int");

                    b.Property<int>("PlayerScore")
                        .HasColumnType("int");

                    b.HasKey("ScoreId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerId_1");

                    b.ToTable("Score", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Turn", b =>
                {
                    b.Property<int>("TurnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TurnId"));

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("CardId_1")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("TurnNumber")
                        .HasColumnType("int");

                    b.HasKey("TurnId");

                    b.HasIndex("CardId");

                    b.HasIndex("CardId_1");

                    b.ToTable("Turn", (string)null);
                });

            modelBuilder.Entity("back_end.Models.GameDeck", b =>
                {
                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("CardId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("CardOrder")
                        .HasColumnType("int");

                    b.Property<int>("CardState")
                        .HasColumnType("int");

                    b.HasKey("PlayerId", "CardId", "GameId");

                    b.HasIndex("CardId");

                    b.HasIndex("GameId");

                    b.ToTable("GameDeck", (string)null);
                });

            modelBuilder.Entity("CatAclysmeApp.Models.BoardSlot", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId");

                    b.HasOne("CatAclysmeApp.Models.Game", "Game")
                        .WithMany("Board")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Build", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Deck", "Deck")
                        .WithMany()
                        .HasForeignKey("DeckId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Deck", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Player", "Player")
                        .WithOne("Deck")
                        .HasForeignKey("CatAclysmeApp.Models.Deck", "PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Game", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Player", "Player_1")
                        .WithMany()
                        .HasForeignKey("PlayerId_1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Player_1");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.GameCard", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Score", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Player", "Player_1")
                        .WithMany()
                        .HasForeignKey("PlayerId_1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Player_1");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Turn", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Card", "Card_1")
                        .WithMany()
                        .HasForeignKey("CardId_1")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Card_1");
                });

            modelBuilder.Entity("back_end.Models.GameDeck", b =>
                {
                    b.HasOne("CatAclysmeApp.Models.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CatAclysmeApp.Models.Player", "Player")
                        .WithMany("GameDecks")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Game");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Game", b =>
                {
                    b.Navigation("Board");
                });

            modelBuilder.Entity("CatAclysmeApp.Models.Player", b =>
                {
                    b.Navigation("Deck")
                        .IsRequired();

                    b.Navigation("GameDecks");
                });
#pragma warning restore 612, 618
        }
    }
}
