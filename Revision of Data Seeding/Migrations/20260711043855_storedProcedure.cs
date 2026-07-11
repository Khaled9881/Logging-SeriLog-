using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Revision_of_Data_Seeding.Migrations
{
    /// <inheritdoc />
    public partial class storedProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
               CREATE PROCEDURE GETALLPERSONS
               AS
               BEGIN
                   SELECT * FROM Person
               END
            ");

            migrationBuilder.Sql(@"
            CREATE PROCEDURE GETPERSONBYID
            @PersonID UNIQUEIDENTIFIER
            AS
            BEGIN
                SELECT * FROM Person WHERE PersonID = @PersonID
            END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE GETALLPERSONS");
            migrationBuilder.Sql(@"DROP PROCEDURE GETPERSONBYID");

        }
    }
}
