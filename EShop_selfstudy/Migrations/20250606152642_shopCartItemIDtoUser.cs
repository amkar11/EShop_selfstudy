using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop_selfstudy.Migrations
{
    /// <inheritdoc />
    public partial class shopCartItemIDtoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "itemId",
                table: "ShopCartItem",
                newName: "shopCartItemId");

            migrationBuilder.AddColumn<int>(
                name: "orderId",
                table: "ShopCartItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shopCartItemId",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCartItem_orderId",
                table: "ShopCartItem",
                column: "orderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCartItem_Order_orderId",
                table: "ShopCartItem",
                column: "orderId",
                principalTable: "Order",
                principalColumn: "orderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopCartItem_Order_orderId",
                table: "ShopCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopCartItem_orderId",
                table: "ShopCartItem");

            migrationBuilder.DropColumn(
                name: "orderId",
                table: "ShopCartItem");

            migrationBuilder.DropColumn(
                name: "shopCartItemId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "shopCartItemId",
                table: "ShopCartItem",
                newName: "itemId");
        }
    }
}
