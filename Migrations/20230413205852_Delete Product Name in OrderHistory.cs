using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Migrations
{
    public partial class DeleteProductNameinOrderHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewProductName",
                table: "OrderHistory");

            migrationBuilder.DropColumn(
                name: "OldProductName",
                table: "OrderHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewProductName",
                table: "OrderHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OldProductName",
                table: "OrderHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
