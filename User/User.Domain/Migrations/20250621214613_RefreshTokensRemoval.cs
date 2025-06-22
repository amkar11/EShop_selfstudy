using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Domain.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokensRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    RefreshToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserDbId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    isRevoked = table.Column<bool>(type: "bit", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserDbId",
                table: "RefreshTokens",
                column: "UserDbId");
        }
    }
}
