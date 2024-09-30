using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardSlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BoardSlot",
                columns: table => new
                {
                    BoardSlotId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardSlot", x => x.BoardSlotId);
                    table.ForeignKey(
                        name: "FK_BoardSlot_Card_CardId",
                        column: x => x.CardId,
                        principalTable: "Card",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_BoardSlot_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "GameId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardSlot_CardId",
                table: "BoardSlot",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardSlot_GameId",
                table: "BoardSlot",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardSlot");
        }
    }
}
