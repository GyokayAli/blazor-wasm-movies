using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BlazorMovies.Server.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO AspNetRoles(Id, [Name], NormalizedName) VALUES('{Guid.NewGuid()}', 'Admin', 'Admin');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
