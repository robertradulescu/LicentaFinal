using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Migrations
{
    public partial class UpdateOrderwithnewfieldsforcustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdresaCumparator",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "CnpCumparator",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "NrInregistrareComert",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeFirma",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdresaCumparator",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CnpCumparator",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NrInregistrareComert",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "NumeFirma",
                table: "Order");
        }
    }
}
