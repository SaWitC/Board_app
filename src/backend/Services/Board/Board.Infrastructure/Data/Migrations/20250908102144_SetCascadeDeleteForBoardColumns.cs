using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class SetCascadeDeleteForBoardColumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BoardColumns_Boards_BoardId",
            table: "BoardColumns");

        migrationBuilder.AddForeignKey(
            name: "FK_BoardColumns_Boards_BoardId",
            table: "BoardColumns",
            column: "BoardId",
            principalTable: "Boards",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_BoardColumns_Boards_BoardId",
            table: "BoardColumns");

        migrationBuilder.AddForeignKey(
            name: "FK_BoardColumns_Boards_BoardId",
            table: "BoardColumns",
            column: "BoardId",
            principalTable: "Boards",
            principalColumn: "Id");
    }
}
