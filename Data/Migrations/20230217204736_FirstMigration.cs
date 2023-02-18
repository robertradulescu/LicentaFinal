using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stocuri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeProdus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantitate = table.Column<long>(type: "bigint", nullable: false),
                    Pret = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocuri", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stocuri");
        }
    }
}
