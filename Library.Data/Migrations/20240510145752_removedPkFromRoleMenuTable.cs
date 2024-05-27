using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class removedPkFromRoleMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenuPermission",
                table: "RoleMenuPermission");

            migrationBuilder.DropIndex(
                name: "IX_RoleMenuPermission_RoleId",
                table: "RoleMenuPermission");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoleMenuPermission");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenuPermission",
                table: "RoleMenuPermission",
                columns: new[] { "RoleId", "NavigationMenuId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenuPermission",
                table: "RoleMenuPermission");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "RoleMenuPermission",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenuPermission",
                table: "RoleMenuPermission",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_RoleId",
                table: "RoleMenuPermission",
                column: "RoleId");
        }
    }
}
