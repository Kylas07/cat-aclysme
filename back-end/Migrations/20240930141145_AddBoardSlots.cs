using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardSlot_Game_GameId",
                table: "BoardSlot");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "BoardSlot",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoardSlot_Game_GameId",
                table: "BoardSlot",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardSlot_Game_GameId",
                table: "BoardSlot");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "BoardSlot",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardSlot_Game_GameId",
                table: "BoardSlot",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameId");
        }
    }
}
