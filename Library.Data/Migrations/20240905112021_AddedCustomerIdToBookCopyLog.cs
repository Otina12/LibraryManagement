using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCustomerIdToBookCopyLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BookCopyLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "BookCopyLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnnualReportRow",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    January = table.Column<int>(type: "int", nullable: false),
                    February = table.Column<int>(type: "int", nullable: false),
                    March = table.Column<int>(type: "int", nullable: false),
                    April = table.Column<int>(type: "int", nullable: false),
                    May = table.Column<int>(type: "int", nullable: false),
                    June = table.Column<int>(type: "int", nullable: false),
                    July = table.Column<int>(type: "int", nullable: false),
                    August = table.Column<int>(type: "int", nullable: false),
                    September = table.Column<int>(type: "int", nullable: false),
                    October = table.Column<int>(type: "int", nullable: false),
                    November = table.Column<int>(type: "int", nullable: false),
                    December = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "BooksDamagedReportRow",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookCopiesDamaged = table.Column<int>(type: "int", nullable: false),
                    BookCopiesLost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "PopularityReportRow",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalBookCopiesReserved = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookCopyLogs_CustomerId",
                table: "BookCopyLogs",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookCopyLogs_Customers_CustomerId",
                table: "BookCopyLogs",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookCopyLogs_Customers_CustomerId",
                table: "BookCopyLogs");

            migrationBuilder.DropTable(
                name: "AnnualReportRow");

            migrationBuilder.DropTable(
                name: "BooksDamagedReportRow");

            migrationBuilder.DropTable(
                name: "PopularityReportRow");

            migrationBuilder.DropIndex(
                name: "IX_BookCopyLogs_CustomerId",
                table: "BookCopyLogs");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "BookCopyLogs");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BookCopyLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
