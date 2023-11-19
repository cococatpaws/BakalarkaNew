using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Books : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    idauthor = table.Column<int>(name: "id_author", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    middlename = table.Column<string>(name: "middle_name", type: "nvarchar(50)", maxLength: 50, nullable: true),
                    surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_authors", x => x.idauthor);
                });

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    idbook = table.Column<int>(name: "id_book", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    quantityinstock = table.Column<int>(name: "quantity_in_stock", type: "int", nullable: false),
                    coverimageurl = table.Column<string>(name: "cover_image_url", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    genre = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    price = table.Column<double>(type: "float", nullable: false),
                    publisher = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    numberofpages = table.Column<int>(name: "number_of_pages", type: "int", nullable: true),
                    bookformat = table.Column<string>(name: "book_format", type: "nvarchar(15)", maxLength: 15, nullable: false),
                    publicationdate = table.Column<DateTime>(name: "publication_date", type: "datetime2", nullable: false),
                    booklanguage = table.Column<string>(name: "book_language", type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.idbook);
                });

            migrationBuilder.CreateTable(
                name: "books_authors",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    authororder = table.Column<int>(name: "author_order", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books_authors", x => new { x.BookId, x.AuthorId });
                    table.ForeignKey(
                        name: "FK_books_authors_authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "authors",
                        principalColumn: "id_author",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_books_authors_books_BookId",
                        column: x => x.BookId,
                        principalTable: "books",
                        principalColumn: "id_book",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_books_authors_AuthorId",
                table: "books_authors",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "books_authors");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "books");
        }
    }
}
