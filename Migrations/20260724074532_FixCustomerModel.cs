using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalPhotoPrintingSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixCustomerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "Customers",
                newName: "Dob");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustId",
                table: "PhotoOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomerCustId",
                table: "PhotoOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EncryptedCreditCardNumber",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FolderName",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModeOfPayment",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "PhotoOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderNumber",
                table: "PhotoOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Customers",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "F_Name",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "L_Name",
                table: "Customers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "P_No",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoOrders_Customers_CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.DropIndex(
                name: "IX_PhotoOrders_CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CustId",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "CustomerCustId",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "EncryptedCreditCardNumber",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "FolderName",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "ModeOfPayment",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderNumber",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "F_Name",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "L_Name",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "P_No",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "Customer",
                newName: "DOB");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoPath",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerName",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustId");
        }
    }
}
