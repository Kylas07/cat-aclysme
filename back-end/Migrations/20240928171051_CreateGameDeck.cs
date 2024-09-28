using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class CreateGameDeck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerHand");

            migrationBuilder.CreateTable(
                name: "GameDeck",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    CardOrder = table.Column<int>(type: "int", nullable: false),
                    CardState = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeck", x => new { x.PlayerId, x.CardId, x.GameId });
                    table.ForeignKey(
                        name: "FK_GameDeck_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDeck_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDeck_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameDeck_CardId",
                table: "GameDeck",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDeck_GameId",
                table: "GameDeck",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameDeck");

            migrationBuilder.CreateTable(
                name: "PlayerHand",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    CardAmount = table.Column<int>(type: "int", nullable: false),
                    PlayerHandId = table.Column<int>(type: "int", nullable: false),
                    PositionInHand = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerHand", x => new { x.GameId, x.CardId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_PlayerHand_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "CardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerHand_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerHand_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerHand_CardId",
                table: "PlayerHand",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerHand_PlayerId",
                table: "PlayerHand",
                column: "PlayerId");
        }
    }
}
