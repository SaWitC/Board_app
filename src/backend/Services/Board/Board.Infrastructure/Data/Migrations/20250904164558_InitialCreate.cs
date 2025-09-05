using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Data.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Boards",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                Users = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Admins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Owners = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ModificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Boards", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BoardColumns",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardColumns", x => x.Id);
                table.ForeignKey(
                    name: "FK_BoardColumns_Boards_BoardId",
                    column: x => x.BoardId,
                    principalTable: "Boards",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "BoardItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                BoardColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ModificationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Priority = table.Column<int>(type: "int", nullable: false),
                AssigneeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_BoardItems_BoardColumns_BoardColumnId",
                    column: x => x.BoardColumnId,
                    principalTable: "BoardColumns",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                BoardItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tags", x => x.Id);
                table.ForeignKey(
                    name: "FK_Tags_BoardItems_BoardItemId",
                    column: x => x.BoardItemId,
                    principalTable: "BoardItems",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_BoardColumns_BoardId",
            table: "BoardColumns",
            column: "BoardId");

        migrationBuilder.CreateIndex(
            name: "IX_BoardItems_BoardColumnId",
            table: "BoardItems",
            column: "BoardColumnId");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_BoardItemId",
            table: "Tags",
            column: "BoardItemId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Tags");

        migrationBuilder.DropTable(
            name: "BoardItems");

        migrationBuilder.DropTable(
            name: "BoardColumns");

        migrationBuilder.DropTable(
            name: "Boards");
    }
}
