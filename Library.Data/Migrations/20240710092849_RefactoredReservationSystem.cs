using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoredReservationSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_BookCopies_BookCopyId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ActualReturnDate",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ReturnCustomerId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "BookCopyId",
                table: "Reservations",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_BookCopyId",
                table: "Reservations",
                newName: "IX_Reservations_BookId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ReservationCopies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookCopyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReturnCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationCopies_BookCopies_BookCopyId",
                        column: x => x.BookCopyId,
                        principalTable: "BookCopies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationCopies_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCopies_BookCopyId",
                table: "ReservationCopies",
                column: "BookCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationCopies_ReservationId",
                table: "ReservationCopies",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Books_BookId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "ReservationCopies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_CustomerId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Reservations",
                newName: "BookCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_BookId",
                table: "Reservations",
                newName: "IX_Reservations_BookCopyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualReturnDate",
                table: "Reservations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnCustomerId",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                columns: new[] { "CustomerId", "BookCopyId", "ReservationDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_BookCopies_BookCopyId",
                table: "Reservations",
                column: "BookCopyId",
                principalTable: "BookCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
