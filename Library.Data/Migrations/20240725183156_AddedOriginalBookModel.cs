using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedOriginalBookModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalBookId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OriginalBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalBooks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_OriginalBookId",
                table: "Books",
                column: "OriginalBookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_OriginalBooks_OriginalBookId",
                table: "Books",
                column: "OriginalBookId",
                principalTable: "OriginalBooks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_OriginalBooks_OriginalBookId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "OriginalBooks");

            migrationBuilder.DropIndex(
                name: "IX_Books_OriginalBookId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "OriginalBookId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
