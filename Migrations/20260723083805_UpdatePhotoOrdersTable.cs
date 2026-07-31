using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPhotoPrintingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoOrdersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "PhotoOrders");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "PhotoOrders",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "PhotoOrders",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PrintSize",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "PhotoOrders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "PhotoOrders");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "PhotoOrders",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PhotoOrders",
                newName: "OrderId");

            migrationBuilder.AlterColumn<string>(
                name: "PrintSize",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
