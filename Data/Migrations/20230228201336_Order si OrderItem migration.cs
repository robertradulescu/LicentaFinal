using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Data.Migrations
{
    public partial class OrdersiOrderItemmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numar = table.Column<int>(type: "int", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cumparator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Iban = table.Column<long>(type: "bigint", nullable: false),
                    Banca = table.Column<long>(type: "bigint", nullable: false),
                    AdresaMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observatii = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeProdus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantitate = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
