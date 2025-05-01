using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace flashcard.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.TopicId);
                });

            migrationBuilder.CreateTable(
                name: "Flashcards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TopicId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnglishWord = table.Column<string>(type: "TEXT", nullable: false),
                    DanishWord = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Flashcards_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "TopicId", "Level", "Name" },
                values: new object[] { 1, 1, "Food" });

            migrationBuilder.InsertData(
                table: "Flashcards",
                columns: new[] { "Id", "DanishWord", "EnglishWord", "TopicId" },
                values: new object[,]
                {
                    { 1, "brød", "bread", 1 },
                    { 2, "ost", "cheese", 1 },
                    { 3, "mælk", "milk", 1 },
                    { 4, "æble", "apple", 1 },
                    { 5, "banan", "banana", 1 },
                    { 6, "kød", "meat", 1 },
                    { 7, "fisk", "fish", 1 },
                    { 8, "vand", "water", 1 },
                    { 9, "salt", "salt", 1 },
                    { 10, "smør", "butter", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_TopicId",
                table: "Flashcards",
                column: "TopicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcards");

            migrationBuilder.DropTable(
                name: "Topics");
        }
    }
}
