using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewPublisher1Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Publishers_PublisherId",
                table: "BookCopies");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_PublisherId",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "BookCopies");

            migrationBuilder.AddColumn<Guid>(
                name: "Publisher1Id",
                table: "BookCopies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Publishers1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers1", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_Publisher1Id",
                table: "BookCopies",
                column: "Publisher1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Publishers1_Publisher1Id",
                table: "BookCopies",
                column: "Publisher1Id",
                principalTable: "Publishers1",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Publishers1_Publisher1Id",
                table: "BookCopies");

            migrationBuilder.DropTable(
                name: "Publishers1");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_Publisher1Id",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "Publisher1Id",
                table: "BookCopies");

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "BookCopies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publishers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_PublisherId",
                table: "BookCopies",
                column: "PublisherId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopies_Publishers_PublisherId",
                table: "BookCopies",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
