using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop_selfstudy.Migrations
{
    /// <inheritdoc />
    public partial class OldCartRemoval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShopCartItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShopCartItem",
                columns: table => new
                {
                    shopCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    carId = table.Column<int>(type: "int", nullable: false),
                    orderId = table.Column<int>(type: "int", nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    shopCartId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopCartItem", x => x.shopCartItemId);
                    table.ForeignKey(
                        name: "FK_ShopCartItem_Car_carId",
                        column: x => x.carId,
                        principalTable: "Car",
                        principalColumn: "carId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopCartItem_Order_orderId",
                        column: x => x.orderId,
                        principalTable: "Order",
                        principalColumn: "orderId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopCartItem_carId",
                table: "ShopCartItem",
                column: "carId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopCartItem_orderId",
                table: "ShopCartItem",
                column: "orderId");
        }
    }
}
