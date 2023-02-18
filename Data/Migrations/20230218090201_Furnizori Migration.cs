using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LicentaFinal.Data.Migrations
{
    public partial class FurnizoriMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Furnizori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Furnizor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CIF = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodRegistruComert = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumarTelefon = table.Column<long>(type: "bigint", nullable: false),
                    AdresaSediu = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnizori", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Furnizori");
        }
    }
}
