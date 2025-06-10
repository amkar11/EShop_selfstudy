using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserDbId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserDbId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserDbId",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isRevoked = table.Column<bool>(type: "bit", nullable: false),
                    UserDbId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.RefreshToken);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserDbId",
                        column: x => x.UserDbId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RoleUserDb",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UserDbId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserDb", x => new { x.RolesId, x.UserDbId });
                    table.ForeignKey(
                        name: "FK_RoleUserDb_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUserDb_Users_UserDbId",
                        column: x => x.UserDbId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserDbId",
                table: "RefreshTokens",
                column: "UserDbId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUserDb_UserDbId",
                table: "RoleUserDb",
                column: "UserDbId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleUserDb");

            migrationBuilder.AddColumn<int>(
                name: "UserDbId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserDbId",
                table: "Roles",
                column: "UserDbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserDbId",
                table: "Roles",
                column: "UserDbId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
