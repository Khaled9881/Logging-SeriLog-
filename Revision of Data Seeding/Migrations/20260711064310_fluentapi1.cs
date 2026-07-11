using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revision_of_Data_Seeding.Migrations
{
    /// <inheritdoc />
    public partial class fluentapi1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxIdNumber",
                table: "Country",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryID",
                keyValue: new Guid("4009667f-7000-4069-b31d-b153d0325e26"),
                column: "TaxIdNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryID",
                keyValue: new Guid("40c3acb3-1855-4e9d-aad0-62f8425c37ae"),
                column: "TaxIdNumber",
                value: "ABC12345");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryID",
                keyValue: new Guid("be699755-747b-41a6-8072-576151c53ab9"),
                column: "TaxIdNumber",
                value: "ABC12345");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxIdNumber",
                table: "Country");
        }
    }
}
