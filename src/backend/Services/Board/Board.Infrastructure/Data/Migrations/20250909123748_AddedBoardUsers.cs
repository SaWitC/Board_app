using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class AddedBoardUsers : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Admins",
            table: "Boards");

        migrationBuilder.DropColumn(
            name: "Owners",
            table: "Boards");

        migrationBuilder.DropColumn(
            name: "Users",
            table: "Boards");

        migrationBuilder.CreateTable(
            name: "BoardUser",
            columns: table => new
            {
                Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Role = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardUser", x => new { x.BoardId, x.Email });
                table.ForeignKey(
                    name: "FK_BoardUser_Boards_BoardId",
                    column: x => x.BoardId,
                    principalTable: "Boards",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_BoardUsers_BoardId",
            table: "BoardUser",
            column: "BoardId");

        migrationBuilder.CreateIndex(
            name: "IX_BoardUsers_Email",
            table: "BoardUser",
            column: "Email");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BoardUser");

        migrationBuilder.AddColumn<string>(
            name: "Admins",
            table: "Boards",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Owners",
            table: "Boards",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Users",
            table: "Boards",
            type: "nvarchar(max)",
            nullable: true);
    }
}
