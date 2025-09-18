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
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                ModificationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Boards", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "BoardColumns",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                BoardId = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardColumns", x => x.Id);
                table.ForeignKey(
                    name: "FK_BoardColumns_Boards_BoardId",
                    column: x => x.BoardId,
                    principalTable: "Boards",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BoardTemplates",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                IsActive = table.Column<bool>(type: "boolean", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BoardTemplates", x => x.Id);
                table.ForeignKey(
                    name: "FK_BoardTemplates_Boards_Id",
                    column: x => x.Id,
                    principalTable: "Boards",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "BoardUser",
            columns: table => new
            {
                Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                BoardId = table.Column<Guid>(type: "uuid", nullable: false),
                Role = table.Column<int>(type: "integer", nullable: false)
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

        migrationBuilder.CreateTable(
            name: "BoardItems",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                Description = table.Column<string>(type: "character varying(1000000)", maxLength: 1000000, nullable: false),
                BoardColumnId = table.Column<Guid>(type: "uuid", nullable: false),
                ModificationDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                Priority = table.Column<int>(type: "integer", nullable: false),
                AssigneeId = table.Column<Guid>(type: "uuid", nullable: false),
                DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CreatedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                TaskType = table.Column<int>(type: "integer", nullable: false, defaultValue: 2)
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
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                BoardItemId = table.Column<Guid>(type: "uuid", nullable: true)
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
            name: "IX_BoardUsers_BoardId",
            table: "BoardUser",
            column: "BoardId");

        migrationBuilder.CreateIndex(
            name: "IX_BoardUsers_Email",
            table: "BoardUser",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "IX_Tags_BoardItemId",
            table: "Tags",
            column: "BoardItemId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "BoardTemplates");

        migrationBuilder.DropTable(
            name: "BoardUser");

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
