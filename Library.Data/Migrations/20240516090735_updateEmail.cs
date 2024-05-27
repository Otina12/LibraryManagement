using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailModel",
                table: "EmailModel");

            migrationBuilder.RenameTable(
                name: "EmailModel",
                newName: "EmailModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailModels",
                table: "EmailModels",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailModels",
                table: "EmailModels");

            migrationBuilder.RenameTable(
                name: "EmailModels",
                newName: "EmailModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailModel",
                table: "EmailModel",
                column: "Id");
        }
    }
}
