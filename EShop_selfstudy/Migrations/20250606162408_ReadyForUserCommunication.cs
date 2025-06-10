using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop_selfstudy.Migrations
{
    /// <inheritdoc />
    public partial class ReadyForUserCommunication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShopCartId",
                table: "ShopCartItem",
                newName: "shopCartId");

            migrationBuilder.AlterColumn<int>(
                name: "shopCartItemId",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "shopCartId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "shopCartId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "shopCartId",
                table: "ShopCartItem",
                newName: "ShopCartId");

            migrationBuilder.AlterColumn<string>(
                name: "shopCartItemId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
