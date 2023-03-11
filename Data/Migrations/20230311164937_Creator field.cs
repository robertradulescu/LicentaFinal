using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Data.Migrations
{
    public partial class Creatorfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Order");

        }
    }
}
