using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revision_of_Data_Seeding.Migrations
{
    /// <inheritdoc />
    public partial class sp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            
            CREATE PROCEDURE INSERTCOUNTRY
            @CountryID UNIQUEIDENTIFIER,
            @CountryName NVARCHAR(MAX)
            AS
            BEGIN
                INSERT INTO Country (CountryID, CountryName)
                VALUES (@CountryID, @CountryName)
            END 

");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE INSERTCOUNTRY");
        }
    }
}
