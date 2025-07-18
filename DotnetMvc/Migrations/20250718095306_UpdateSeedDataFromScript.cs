using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetMvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedDataFromScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add additional Categories from database_script.sql
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 6, "Comic books with action-packed adventures", "Action/Adventure" },
                    { 7, "Comic books featuring detective work and mysteries", "Mystery" },
                    { 8, "Comic books set in historical time periods", "Historical" },
                    { 9, "Comic books depicting everyday experiences", "Slice of Life" },
                    { 10, "Comic books with humorous content", "Comedy" }
                });

            // Add additional ComicBooks from database_script.sql
            migrationBuilder.InsertData(
                table: "ComicBooks",
                columns: new[] { "Id", "Artist", "Author", "CategoryId", "CoverImageUrl", "CreatedDate", "Description", "IsAvailable", "PageCount", "Price", "PublicationDate", "Title" },
                values: new object[,]
                {
                    { 6, "Dave Johnson", "Mark Millar", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "What if Superman had been raised in the Soviet Union?", true, 48, 17.99m, new DateTime(2003, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Superman: Red Son" },
                    { 7, "Masashi Kishimoto", "Masashi Kishimoto", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Young ninja Naruto Uzumaki seeks recognition and dreams of becoming the Hokage", true, 192, 10.99m, new DateTime(1999, 9, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Naruto" },
                    { 8, "Dave Gibbons", "Alan Moore", 1, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "An alternate history where superheroes emerged in the 1940s and 1960s", true, 416, 29.99m, new DateTime(1986, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Watchmen" },
                    { 9, "Hajime Isayama", "Hajime Isayama", 2, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Humanity fights for survival against man-eating giants", true, 192, 12.99m, new DateTime(2009, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Attack on Titan" },
                    { 10, "Dev Pramanik", "Brian Herbert", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adaptation of the sci-fi novel set in the Dune universe", true, 32, 16.99m, new DateTime(2020, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Dune: House Atreides" },
                    { 11, "Art Spiegelman", "Art Spiegelman", 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A survivor's tale of the Holocaust with Jews depicted as mice and Nazis as cats", true, 296, 15.99m, new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Maus" },
                    { 12, "Marjane Satrapi", "Marjane Satrapi", 8, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Autobiographical comic about growing up in Iran during the Islamic Revolution", true, 160, 14.99m, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Persepolis" },
                    { 13, "Bryan Lee O'Malley", "Bryan Lee O'Malley", 9, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Scott must defeat his new girlfriend's seven evil exes", true, 168, 11.99m, new DateTime(2004, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Scott Pilgrim" },
                    { 14, "Jeff Smith", "Jeff Smith", 4, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Three cousins get lost in a mysterious valley filled with wonderful and terrifying creatures", true, 140, 12.99m, new DateTime(1991, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bone" },
                    { 15, "Mike Mignola", "Mike Mignola", 5, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A demon summoned by Nazis works as a paranormal investigator", true, 128, 18.99m, new DateTime(1993, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Hellboy" },
                    { 16, "Pia Guerra", "Brian K. Vaughan", 3, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A mysterious plague kills all male mammals except one man and his monkey", true, 60, 15.99m, new DateTime(2002, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Y: The Last Man" },
                    { 17, "Albert Uderzo", "René Goscinny", 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adventures of Gaulish warriors resisting Roman occupation in 50 BCE", true, 48, 13.99m, new DateTime(1959, 10, 29, 0, 0, 0, 0, DateTimeKind.Utc), "Asterix" },
                    { 18, "Hergé", "Hergé", 6, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adventures of young Belgian reporter and his dog Snowy", true, 62, 14.99m, new DateTime(1929, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), "Tintin" },
                    { 19, "Juanjo Guarnido", "Juan Díaz Canales", 7, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Anthropomorphic animals in a noir detective setting", true, 56, 19.99m, new DateTime(2000, 11, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Blacksad" },
                    { 20, "Bill Watterson", "Bill Watterson", 10, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Adventures of a young boy and his imaginary tiger friend", true, 128, 16.99m, new DateTime(1985, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "Calvin and Hobbes" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove additional ComicBooks
            migrationBuilder.DeleteData(
                table: "ComicBooks",
                keyColumn: "Id",
                keyValues: new object[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

            // Remove additional Categories
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValues: new object[] { 6, 7, 8, 9, 10 });
        }
    }
}
