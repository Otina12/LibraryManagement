using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class MovedGenresToOriginalBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookGenre");

            migrationBuilder.CreateTable(
                name: "OriginalBookGenre",
                columns: table => new
                {
                    OriginalBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalBookGenre", x => new { x.OriginalBookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_OriginalBookGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OriginalBookGenre_OriginalBooks_OriginalBookId",
                        column: x => x.OriginalBookId,
                        principalTable: "OriginalBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OriginalBookGenre_GenreId",
                table: "OriginalBookGenre",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OriginalBookGenre");

            migrationBuilder.CreateTable(
                name: "BookGenre",
                columns: table => new
                {
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookGenre", x => new { x.BookId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_BookGenre_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookGenre_GenreId",
                table: "BookGenre",
                column: "GenreId");
        }
    }
}
