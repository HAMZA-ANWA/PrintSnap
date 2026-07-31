using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPhotoPrintingSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixPhotoOrderProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoOrders_Customers_CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.DropIndex(
                name: "IX_PhotoOrders_CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "PhotoOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoOrders_CustId",
                table: "PhotoOrders",
                column: "CustId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoOrders_Customers_CustId",
                table: "PhotoOrders",
                column: "CustId",
                principalTable: "Customers",
                principalColumn: "CustId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoOrders_Customers_CustId",
                table: "PhotoOrders");

            migrationBuilder.DropIndex(
                name: "IX_PhotoOrders_CustId",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "PhotoOrders");

            migrationBuilder.AddColumn<int>(
                name: "CustomerCustId",
                table: "PhotoOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhotoOrders_CustomerCustId",
                table: "PhotoOrders",
                column: "CustomerCustId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoOrders_Customers_CustomerCustId",
                table: "PhotoOrders",
                column: "CustomerCustId",
                principalTable: "Customers",
                principalColumn: "CustId");
        }
    }
}
