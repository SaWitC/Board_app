using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBoardTemplateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardTemplates_Boards_BoardId",
                table: "BoardTemplates");

            migrationBuilder.DropIndex(
                name: "IX_BoardTemplates_BoardId",
                table: "BoardTemplates");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "BoardTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardTemplates_Boards_Id",
                table: "BoardTemplates",
                column: "Id",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoardTemplates_Boards_Id",
                table: "BoardTemplates");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "BoardTemplates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BoardTemplates_BoardId",
                table: "BoardTemplates",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_BoardTemplates_Boards_BoardId",
                table: "BoardTemplates",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
