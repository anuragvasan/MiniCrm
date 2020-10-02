using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MiniCrm.DataModel.Migrations
{
    public partial class CustomerSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "AddressLine1", "AddressLine2", "City", "Email", "Name", "PhoneExtension", "PhoneNumber", "PostalCode", "State" },
                values: new object[] { new Guid("69769d59-cf07-4381-93b1-1e8c08a1aaf7"), "123 Main Street", "Apt 321", "Anywhere", "test.customer@example.com", "Test Customer", "1234", "(123) 123-1234", "12345", "MA" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: new Guid("69769d59-cf07-4381-93b1-1e8c08a1aaf7"));
        }
    }
}
