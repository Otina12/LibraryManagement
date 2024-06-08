using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredBookAndBookCopy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopies_Publishers_PublisherId",
                table: "BookCopies");

            migrationBuilder.DropTable(
                name: "ShelfGenre");

            migrationBuilder.DropTable(
                name: "ShelfGenres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres");

            migrationBuilder.DropIndex(
                name: "IX_BookGenres_BookId",
                table: "BookGenres");

            migrationBuilder.DropIndex(
                name: "IX_BookCopies_PublisherId",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "BookCopies");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "BookCopies");

            migrationBuilder.AddColumn<int>(
                name: "Edition",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PublisherId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres",
                columns: new[] { "BookId", "GenreId" });

            migrationBuilder.CreateIndex(
                name: "IX_Books_PublisherId",
                table: "Books",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres");

            migrationBuilder.DropIndex(
                name: "IX_BookGenres_GenreId",
                table: "BookGenres");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PageCount",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "Edition",
                table: "BookCopies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PageCount",
                table: "BookCopies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PublisherId",
                table: "BookCopies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookGenres",
                table: "BookGenres",
                columns: new[] { "GenreId", "BookId" });

            migrationBuilder.CreateTable(
                name: "ShelfGenre",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    ShelvesRoomId = table.Column<int>(type: "int", nullable: false),
                    ShelvesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelfGenre", x => new { x.GenresId, x.ShelvesRoomId, x.ShelvesId });
                    table.ForeignKey(
                        name: "FK_ShelfGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShelfGenre_Shelves_ShelvesRoomId_ShelvesId",
                        columns: x => new { x.ShelvesRoomId, x.ShelvesId },
                        principalTable: "Shelves",
                        principalColumns: new[] { "RoomId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShelfGenres",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    ShelfId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelfGenres", x => new { x.GenreId, x.ShelfId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_ShelfGenres_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShelfGenres_Shelves_ShelfId_RoomId",
                        columns: x => new { x.ShelfId, x.RoomId },
                        principalTable: "Shelves",
                        principalColumns: new[] { "RoomId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenres_BookId",
                table: "BookGenres",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookCopies_PublisherId",
                table: "BookCopies",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_ShelfGenre_ShelvesRoomId_ShelvesId",
                table: "ShelfGenre",
                columns: new[] { "ShelvesRoomId", "ShelvesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShelfGenres_ShelfId_RoomId",
                table: "ShelfGenres",
                columns: new[] { "ShelfId", "RoomId" });

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
