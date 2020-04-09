using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMovies.Server.Migrations
{
    public partial class MovieTheaters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Theaters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theaters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoviesTheaters",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    TheaterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviesTheaters", x => new { x.MovieId, x.TheaterId });
                    table.ForeignKey(
                        name: "FK_MoviesTheaters_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviesTheaters_Theaters_TheaterId",
                        column: x => x.TheaterId,
                        principalTable: "Theaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Theaters",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { 1, "Spui 191 The Hague", "Filmhuis Den Haag" },
                    { 2, "Pres. Kennedylaan 5 The Hague", "Omniversum" },
                    { 3, "Buitenhof 20 The Hague", "Pathé Buitenhof" },
                    { 4, "Kurhausweg 2 The Hague", "Pathé Scheveningen" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoviesTheaters_TheaterId",
                table: "MoviesTheaters",
                column: "TheaterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviesTheaters");

            migrationBuilder.DropTable(
                name: "Theaters");
        }
    }
}
