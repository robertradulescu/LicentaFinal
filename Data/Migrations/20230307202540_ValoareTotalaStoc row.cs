using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Data.Migrations
{
    public partial class ValoareTotalaStocrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValoareTotalaStoc",
                table: "OrderItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValoareTotalaStoc",
                table: "OrderItem");
        }
    }
}
