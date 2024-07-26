using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedOriginalBookToBaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "OriginalBooks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                table: "OriginalBooks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalPublishYear",
                table: "OriginalBooks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "OriginalBooks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "OriginalBooks");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                table: "OriginalBooks");

            migrationBuilder.DropColumn(
                name: "OriginalPublishYear",
                table: "OriginalBooks");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "OriginalBooks");
        }
    }
}
