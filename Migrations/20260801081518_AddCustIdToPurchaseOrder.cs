using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPhotoPrintingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCustIdToPurchaseOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "PurchaseOrders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25);

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "PurchaseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CustId",
                table: "PurchaseOrders",
                column: "CustId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Customers_CustId",
                table: "PurchaseOrders",
                column: "CustId",
                principalTable: "Customers",
                principalColumn: "CustId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Customers_CustId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_CustId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerEmail",
                table: "PurchaseOrders",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
