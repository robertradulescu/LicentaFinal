using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Migrations
{
    public partial class CreatorinOrderItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "OrderItem",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "OrderItem");
        }
    }
}
