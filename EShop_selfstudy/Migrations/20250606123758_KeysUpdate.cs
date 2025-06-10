using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop_selfstudy.Migrations
{
    /// <inheritdoc />
    public partial class KeysUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "OrderDetail",
                newName: "orderDetailId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Order",
                newName: "orderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "orderDetailId",
                table: "OrderDetail",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "orderId",
                table: "Order",
                newName: "id");
        }
    }
}
