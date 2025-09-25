using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedBoardItemWithAssigneeAndTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_BoardItems_BoardItemId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_BoardItemId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "BoardItemId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "AssigneeId",
                table: "BoardItems",
                newName: "BoardId");

            migrationBuilder.AddColumn<string>(
                name: "AssigneeEmail",
                table: "BoardItems",
                type: "character varying(100)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BoardItemTag",
                columns: table => new
                {
                    BoardItemsId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardItemTag", x => new { x.BoardItemsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_BoardItemTag_BoardItems_BoardItemsId",
                        column: x => x.BoardItemsId,
                        principalTable: "BoardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardItemTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardItems_BoardId_AssigneeEmail",
                table: "BoardItems",
                columns: new[] { "BoardId", "AssigneeEmail" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardItemTag_TagsId",
                table: "BoardItemTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardItems_BoardUser_BoardId_AssigneeEmail",
                table: "BoardItems",
                columns: new[] { "BoardId", "AssigneeEmail" },
                principalTable: "BoardUser",
                principalColumns: new[] { "BoardId", "Email" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardItems_BoardUser_BoardId_AssigneeEmail",
                table: "BoardItems");

            migrationBuilder.DropTable(
                name: "BoardItemTag");

            migrationBuilder.DropIndex(
                name: "IX_BoardItems_BoardId_AssigneeEmail",
                table: "BoardItems");

            migrationBuilder.DropColumn(
                name: "AssigneeEmail",
                table: "BoardItems");

            migrationBuilder.RenameColumn(
                name: "BoardId",
                table: "BoardItems",
                newName: "AssigneeId");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardItemId",
                table: "Tags",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_BoardItemId",
                table: "Tags",
                column: "BoardItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_BoardItems_BoardItemId",
                table: "Tags",
                column: "BoardItemId",
                principalTable: "BoardItems",
                principalColumn: "Id");
        }
    }
}
