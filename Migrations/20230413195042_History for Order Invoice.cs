using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Migrations
{
    public partial class HistoryforOrderInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "OrderInvoiceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OldCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldSeries = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewSeries = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldNumber = table.Column<int>(type: "int", nullable: false),
                    NewNumber = table.Column<int>(type: "int", nullable: false),
                    OldCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewAdress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldIban = table.Column<long>(type: "bigint", nullable: false),
                    NewIban = table.Column<long>(type: "bigint", nullable: false),
                    OldBank = table.Column<long>(type: "bigint", nullable: false),
                    NewBank = table.Column<long>(type: "bigint", nullable: false),
                    OldAddressMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewAddressMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldObservation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewObservation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldCreator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewCreator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldBuyerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewBuyerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OldCnpBuyer = table.Column<long>(type: "bigint", nullable: false),
                    NewCnpBuyer = table.Column<long>(type: "bigint", nullable: false),
                    OldTradeRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewTradeRegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderInvoiceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderInvoiceHistory_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });



            migrationBuilder.CreateIndex(
                name: "IX_OrderInvoiceHistory_OrderId",
                table: "OrderInvoiceHistory",
                column: "OrderId");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "OrderInvoiceHistory");

 
        }
    }
}
