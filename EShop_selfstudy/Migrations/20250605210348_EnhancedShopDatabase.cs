using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop_selfstudy.Migrations
{
    /// <inheritdoc />
    public partial class EnhancedShopDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Car_CarId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopCartItem_Car_carid",
                table: "ShopCartItem");

            migrationBuilder.RenameColumn(
                name: "carid",
                table: "ShopCartItem",
                newName: "carId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopCartItem_carid",
                table: "ShopCartItem",
                newName: "IX_ShopCartItem_carId");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "OrderDetail",
                newName: "carId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_CarId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_carId");

            migrationBuilder.RenameColumn(
                name: "ordertime",
                table: "Order",
                newName: "orderTime");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "categoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Car",
                newName: "carId");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "Order",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "Order",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Order",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Order",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Order",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Car_carId",
                table: "OrderDetail",
                column: "carId",
                principalTable: "Car",
                principalColumn: "carId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCartItem_Car_carId",
                table: "ShopCartItem",
                column: "carId",
                principalTable: "Car",
                principalColumn: "carId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Car_carId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopCartItem_Car_carId",
                table: "ShopCartItem");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "carId",
                table: "ShopCartItem",
                newName: "carid");

            migrationBuilder.RenameIndex(
                name: "IX_ShopCartItem_carId",
                table: "ShopCartItem",
                newName: "IX_ShopCartItem_carid");

            migrationBuilder.RenameColumn(
                name: "carId",
                table: "OrderDetail",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_carId",
                table: "OrderDetail",
                newName: "IX_OrderDetail_CarId");

            migrationBuilder.RenameColumn(
                name: "orderTime",
                table: "Order",
                newName: "ordertime");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "carId",
                table: "Car",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(35)",
                oldMaxLength: 35);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Car_CarId",
                table: "OrderDetail",
                column: "CarId",
                principalTable: "Car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopCartItem_Car_carid",
                table: "ShopCartItem",
                column: "carid",
                principalTable: "Car",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
