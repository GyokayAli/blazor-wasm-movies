using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMovies.Server.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO AspNetRoles(Id, [Name], NormalizedName)
                                    VALUES('222be8cc-a5b5-40c6-89d0-141eb5ca6bc1', 'Admin', 'Admin');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
