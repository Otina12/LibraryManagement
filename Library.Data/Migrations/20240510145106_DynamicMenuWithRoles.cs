using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Data.Migrations
{
    /// <inheritdoc />
    public partial class DynamicMenuWithRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NavigationMenu",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NavigationMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NavigationMenu_NavigationMenu_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalTable: "NavigationMenu",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleMenuPermission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NavigationMenuId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenuPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermission_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleMenuPermission_NavigationMenu_NavigationMenuId",
                        column: x => x.NavigationMenuId,
                        principalTable: "NavigationMenu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NavigationMenu_ParentMenuId",
                table: "NavigationMenu",
                column: "ParentMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_NavigationMenuId",
                table: "RoleMenuPermission",
                column: "NavigationMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenuPermission_RoleId",
                table: "RoleMenuPermission",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleMenuPermission");

            migrationBuilder.DropTable(
                name: "NavigationMenu");
        }
    }
}
