using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace back_end.Migrations
{
    /// <inheritdoc />
    public partial class AddCardStateProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAttackedThisTurn",
                table: "GameDeck",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlacedPreviousTurn",
                table: "GameDeck",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasAttackedThisTurn",
                table: "BoardSlot",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPlacedPreviousTurn",
                table: "BoardSlot",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAttackedThisTurn",
                table: "GameDeck");

            migrationBuilder.DropColumn(
                name: "IsPlacedPreviousTurn",
                table: "GameDeck");

            migrationBuilder.DropColumn(
                name: "HasAttackedThisTurn",
                table: "BoardSlot");

            migrationBuilder.DropColumn(
                name: "IsPlacedPreviousTurn",
                table: "BoardSlot");
        }
    }
}
