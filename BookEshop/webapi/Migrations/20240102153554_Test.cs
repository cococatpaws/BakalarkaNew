using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
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
                name: "PaymentType",
                columns: table => new
                {
                    idpayment = table.Column<int>(name: "id_payment", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paymenttype = table.Column<string>(name: "payment_type", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    additionalcost = table.Column<double>(name: "additional_cost", type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.idpayment);
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
                name: "ShippingType",
                columns: table => new
                {
                    idshipping = table.Column<int>(name: "id_shipping", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shippingtype = table.Column<string>(name: "shipping_type", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    shippingcost = table.Column<double>(name: "shipping_cost", type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingType", x => x.idshipping);
                });

            migrationBuilder.CreateTable(
                name: "user_type",
                columns: table => new
                {
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_type", x => x.idusertype);
                });

            migrationBuilder.CreateTable(
                name: "billing_address",
                columns: table => new
                {
                    idbillingaddress = table.Column<int>(name: "id_billing_address", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idaddress = table.Column<int>(name: "id_address", type: "int", nullable: false),
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_billing_address_user_type_id_user_type",
                        column: x => x.idusertype,
                        principalTable: "user_type",
                        principalColumn: "id_user_type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shipping_address",
                columns: table => new
                {
                    idshippingaddress = table.Column<int>(name: "id_shipping_address", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    shippingdetails = table.Column<string>(name: "shipping_details", type: "nvarchar(200)", maxLength: 200, nullable: false),
                    idaddress = table.Column<int>(name: "id_address", type: "int", nullable: false),
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_shipping_address_user_type_id_user_type",
                        column: x => x.idusertype,
                        principalTable: "user_type",
                        principalColumn: "id_user_type",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    idorder = table.Column<int>(name: "id_order", type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateplaced = table.Column<DateTime>(name: "date_placed", type: "datetime2", nullable: true),
                    orderstatus = table.Column<string>(name: "order_status", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ordertype = table.Column<string>(name: "order_type", type: "nvarchar(20)", maxLength: 20, nullable: false),
                    orderdetails = table.Column<string>(name: "order_details", type: "nvarchar(200)", maxLength: 200, nullable: true),
                    idshippingtype = table.Column<int>(name: "id_shipping_type", type: "int", nullable: false),
                    idpaymenttype = table.Column<int>(name: "id_payment_type", type: "int", nullable: false),
                    idusertype = table.Column<int>(name: "id_user_type", type: "int", nullable: false),
                    idshippingaddress = table.Column<int>(name: "id_shipping_address", type: "int", nullable: false),
                    idbillingaddress = table.Column<int>(name: "id_billing_address", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.idorder);
                    table.ForeignKey(
                        name: "FK_Order_PaymentType_id_payment_type",
                        column: x => x.idpaymenttype,
                        principalTable: "PaymentType",
                        principalColumn: "id_payment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_ShippingType_id_shipping_type",
                        column: x => x.idshippingtype,
                        principalTable: "ShippingType",
                        principalColumn: "id_shipping",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_billing_address_id_billing_address",
                        column: x => x.idbillingaddress,
                        principalTable: "billing_address",
                        principalColumn: "id_billing_address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_shipping_address_id_shipping_address",
                        column: x => x.idshippingaddress,
                        principalTable: "shipping_address",
                        principalColumn: "id_shipping_address",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_user_type_id_user_type",
                        column: x => x.idusertype,
                        principalTable: "user_type",
                        principalColumn: "id_user_type",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders_Books",
                columns: table => new
                {
                    idorder = table.Column<int>(name: "id_order", type: "int", nullable: false),
                    idbook = table.Column<int>(name: "id_book", type: "int", nullable: false),
                    bookprice = table.Column<double>(name: "book_price", type: "float", nullable: false),
                    quantityordered = table.Column<int>(name: "quantity_ordered", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders_Books", x => new { x.idorder, x.idbook });
                    table.ForeignKey(
                        name: "FK_Orders_Books_Order_id_order",
                        column: x => x.idorder,
                        principalTable: "Order",
                        principalColumn: "id_order",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Books_book_id_book",
                        column: x => x.idbook,
                        principalTable: "book",
                        principalColumn: "id_book",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "id_payment", "additional_cost", "payment_type" },
                values: new object[,]
                {
                    { 1, 0.0, "CardTransfer" },
                    { 2, 1.5, "CashOnDelivery" }
                });

            migrationBuilder.InsertData(
                table: "ShippingType",
                columns: new[] { "id_shipping", "shipping_cost", "shipping_type" },
                values: new object[,]
                {
                    { 1, 4.9900000000000002, "GLS" },
                    { 2, 4.4900000000000002, "Post" },
                    { 3, 4.4900000000000002, "SPS" },
                    { 4, 0.0, "PersonalPickup" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_billing_address_id_address",
                table: "billing_address",
                column: "id_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_billing_address_id_user_type",
                table: "billing_address",
                column: "id_user_type");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_billing_address",
                table: "Order",
                column: "id_billing_address");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_payment_type",
                table: "Order",
                column: "id_payment_type");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_shipping_address",
                table: "Order",
                column: "id_shipping_address");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_shipping_type",
                table: "Order",
                column: "id_shipping_type");

            migrationBuilder.CreateIndex(
                name: "IX_Order_id_user_type",
                table: "Order",
                column: "id_user_type");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Books_id_book",
                table: "Orders_Books",
                column: "id_book");

            migrationBuilder.CreateIndex(
                name: "IX_shipping_address_id_address",
                table: "shipping_address",
                column: "id_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipping_address_id_user_type",
                table: "shipping_address",
                column: "id_user_type");

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
                name: "Orders_Books");

            migrationBuilder.DropTable(
                name: "temporary_user");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "personal_info");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "ShippingType");

            migrationBuilder.DropTable(
                name: "billing_address");

            migrationBuilder.DropTable(
                name: "shipping_address");

            migrationBuilder.DropTable(
                name: "address");

            migrationBuilder.DropTable(
                name: "user_type");

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
