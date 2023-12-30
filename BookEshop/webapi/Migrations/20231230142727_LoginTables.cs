using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class LoginTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_authors_authors_AuthorId",
                table: "books_authors");

            migrationBuilder.DropForeignKey(
                name: "FK_books_authors_books_BookId",
                table: "books_authors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_books",
                table: "books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_authors",
                table: "authors");

            migrationBuilder.RenameTable(
                name: "books",
                newName: "book");

            migrationBuilder.RenameTable(
                name: "authors",
                newName: "author");

            migrationBuilder.AddPrimaryKey(
                name: "PK_book",
                table: "book",
                column: "id_book");

            migrationBuilder.AddPrimaryKey(
                name: "PK_author",
                table: "author",
                column: "id_author");

            migrationBuilder.CreateTable(
                name: "address",
                columns: table => new
                {
                    idaddress = table.Column<int>(name: "id_address", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    country = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    addressnumber = table.Column<string>(name: "address_number", type: "nvarchar(10)", maxLength: 10, nullable: false),
                    postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_address", x => x.idaddress);
                });

            migrationBuilder.CreateTable(
                name: "personal_info",
                columns: table => new
                {
                    idpersonalinfo = table.Column<int>(name: "id_personal_info", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    surname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phonenumber = table.Column<string>(name: "phone_number", type: "nvarchar(13)", maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personal_info", x => x.idpersonalinfo);
                });

            migrationBuilder.CreateTable(
                name: "billing_address",
                columns: table => new
                {
                    idbillingaddress = table.Column<int>(name: "id_billing_address", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idaddress = table.Column<int>(name: "id_address", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billing_address", x => x.idbillingaddress);
                    table.ForeignKey(
                        name: "FK_billing_address_address_id_address",
                        column: x => x.idaddress,
                        principalTable: "address",
                        principalColumn: "id_address",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "shipping_address",
                columns: table => new
                {
                    idshippingaddress = table.Column<int>(name: "id_shipping_address", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shippingdetails = table.Column<string>(name: "shipping_details", type: "nvarchar(200)", maxLength: 200, nullable: false),
                    idaddress = table.Column<int>(name: "id_address", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipping_address", x => x.idshippingaddress);
                    table.ForeignKey(
                        name: "FK_shipping_address_address_id_address",
                        column: x => x.idaddress,
                        principalTable: "address",
                        principalColumn: "id_address",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idshippingadress = table.Column<int>(name: "id_shipping_adress", type: "int", nullable: false),
                    idbillingaddress = table.Column<int>(name: "id_billing_address", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_type", x => x.idusertype);
                    table.ForeignKey(
                        name: "FK_user_type_billing_address_id_billing_address",
                        column: x => x.idbillingaddress,
                        principalTable: "billing_address",
                        principalColumn: "id_billing_address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_type_shipping_address_id_shipping_adress",
                        column: x => x.idshippingadress,
                        principalTable: "shipping_address",
                        principalColumn: "id_shipping_address",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "temporary_user",
                columns: table => new
                {
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false),
                    idpersonalinfo = table.Column<int>(name: "id_personal_info", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_temporary_user", x => x.idusertype);
                    table.ForeignKey(
                        name: "FK_temporary_user_personal_info_id_personal_info",
                        column: x => x.idpersonalinfo,
                        principalTable: "personal_info",
                        principalColumn: "id_personal_info",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_temporary_user_user_type_id_user_type",
                        column: x => x.idusertype,
                        principalTable: "user_type",
                        principalColumn: "id_user_type",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false),
                    idpersonalinfo = table.Column<int>(name: "id_personal_info", type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    profilepictureurl = table.Column<string>(name: "profile_picture_url", type: "nvarchar(255)", maxLength: 255, nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.idusertype);
                    table.ForeignKey(
                        name: "FK_user_personal_info_id_personal_info",
                        column: x => x.idpersonalinfo,
                        principalTable: "personal_info",
                        principalColumn: "id_personal_info",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_user_type_id_user_type",
                        column: x => x.idusertype,
                        principalTable: "user_type",
                        principalColumn: "id_user_type",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_address_id_address",
                table: "billing_address",
                column: "id_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipping_address_id_address",
                table: "shipping_address",
                column: "id_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_temporary_user_id_personal_info",
                table: "temporary_user",
                column: "id_personal_info",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_id_personal_info",
                table: "user",
                column: "id_personal_info",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_type_id_billing_address",
                table: "user_type",
                column: "id_billing_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_type_id_shipping_adress",
                table: "user_type",
                column: "id_shipping_adress",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_books_authors_author_AuthorId",
                table: "books_authors",
                column: "AuthorId",
                principalTable: "author",
                principalColumn: "id_author",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_books_authors_book_BookId",
                table: "books_authors",
                column: "BookId",
                principalTable: "book",
                principalColumn: "id_book",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_authors_author_AuthorId",
                table: "books_authors");

            migrationBuilder.DropForeignKey(
                name: "FK_books_authors_book_BookId",
                table: "books_authors");

            migrationBuilder.DropTable(
                name: "temporary_user");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "personal_info");

            migrationBuilder.DropTable(
                name: "user_type");

            migrationBuilder.DropTable(
                name: "billing_address");

            migrationBuilder.DropTable(
                name: "shipping_address");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_book",
                table: "book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_author",
                table: "author");

            migrationBuilder.RenameTable(
                name: "book",
                newName: "books");

            migrationBuilder.RenameTable(
                name: "author",
                newName: "authors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_books",
                table: "books",
                column: "id_book");

            migrationBuilder.AddPrimaryKey(
                name: "PK_authors",
                table: "authors",
                column: "id_author");

            migrationBuilder.AddForeignKey(
                name: "FK_books_authors_authors_AuthorId",
                table: "books_authors",
                column: "AuthorId",
                principalTable: "authors",
                principalColumn: "id_author",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_books_authors_books_BookId",
                table: "books_authors",
                column: "BookId",
                principalTable: "books",
                principalColumn: "id_book",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
