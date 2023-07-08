using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_API.Migrations
{
    /// <inheritdoc />
    public partial class AddtblBaskets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "tblProductImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "tblProductImages",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "tblProductImages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "tblProductImages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblBaskets",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quintity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBaskets", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_tblBaskets_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblBaskets_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tblBaskets_ProductId",
                table: "tblBaskets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages");

            migrationBuilder.DropTable(
                name: "tblBaskets");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "tblProductImages");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "tblProductImages");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "tblProductImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProductImages_tblProducts_ProductId",
                table: "tblProductImages",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
