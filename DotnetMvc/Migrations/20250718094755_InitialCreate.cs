using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotnetMvc.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComicBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Author = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Artist = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PageCount = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComicBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComicBooks_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Comic books featuring superheroes and their adventures", "Superhero" },
                    { 2, "Japanese comic books and graphic novels", "Manga" },
                    { 3, "Comic books with science fiction themes", "Science Fiction" },
                    { 4, "Comic books with fantasy elements and worlds", "Fantasy" },
                    { 5, "Comic books with horror and supernatural themes", "Horror" }
                });

            migrationBuilder.InsertData(
                table: "ComicBooks",
                columns: new[] { "Id", "Artist", "Author", "CategoryId", "CoverImageUrl", "CreatedDate", "Description", "IsAvailable", "PageCount", "Price", "PublicationDate", "Title" },
                values: new object[,]
                {
                    { 1, "David Mazzucchelli", "Frank Miller", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Batman's early days fighting crime in Gotham City", true, null, 19.99m, null, "Batman: Year One" },
                    { 2, "Eiichiro Oda", "Eiichiro Oda", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Monkey D. Luffy and his pirate crew search for the greatest treasure, the One Piece", true, null, 9.99m, null, "One Piece" },
                    { 3, "Fiona Staples", "Brian K. Vaughan", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Epic space opera/fantasy comic book series", true, null, 14.99m, null, "Saga" },
                    { 4, "Sam Kieth", "Neil Gaiman", 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "The Lord of Dreams has been imprisoned for decades and must reclaim his power", true, null, 24.99m, null, "The Sandman" },
                    { 5, "Junji Ito", "Junji Ito", 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A town is haunted by a supernatural spiral pattern", true, null, 22.99m, null, "Uzumaki" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComicBooks_CategoryId",
                table: "ComicBooks",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComicBooks");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
