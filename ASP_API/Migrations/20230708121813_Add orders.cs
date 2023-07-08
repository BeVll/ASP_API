using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ASP_API.Migrations
{
    /// <inheritdoc />
    public partial class Addorders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblDeliveryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDeliveryTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tblDeliveriesInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DeliveryTypeId = table.Column<int>(type: "int", nullable: false),
                    Oblast = table.Column<string>(type: "longtext", nullable: true),
                    Rayon = table.Column<string>(type: "longtext", nullable: true),
                    City = table.Column<string>(type: "longtext", nullable: true),
                    Street = table.Column<string>(type: "longtext", nullable: true),
                    NumberOfHouse = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDeliveriesInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblDeliveriesInfo_tblDeliveryTypes_DeliveryTypeId",
                        column: x => x.DeliveryTypeId,
                        principalTable: "tblDeliveryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tblOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(type: "longtext", nullable: true),
                    DataCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Payment_Type = table.Column<string>(type: "longtext", nullable: true),
                    Payment_Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Recipient_Full_Name = table.Column<string>(type: "longtext", nullable: true),
                    Recipient_Phone = table.Column<string>(type: "longtext", nullable: true),
                    DeliveryInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblOrders_tblDeliveriesInfo_DeliveryInfoId",
                        column: x => x.DeliveryInfoId,
                        principalTable: "tblDeliveriesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProducts_tblOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tblOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderProducts_tblProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tblProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_OrderId",
                table: "OrderProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_ProductId",
                table: "OrderProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDeliveriesInfo_DeliveryTypeId",
                table: "tblDeliveriesInfo",
                column: "DeliveryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrders_DeliveryInfoId",
                table: "tblOrders",
                column: "DeliveryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrders_UserId",
                table: "tblOrders",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderProducts");

            migrationBuilder.DropTable(
                name: "tblOrders");

            migrationBuilder.DropTable(
                name: "tblDeliveriesInfo");

            migrationBuilder.DropTable(
                name: "tblDeliveryTypes");
        }
    }
}
