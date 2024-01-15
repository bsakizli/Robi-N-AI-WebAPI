using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RBN_SGK_HealthReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RBN_SGK_HealthReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISYERIKODU = table.Column<int>(type: "int", nullable: false),
                    TCKIMLIKNO = table.Column<long>(type: "bigint", nullable: false),
                    AD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SOYAD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MEDULARAPORID = table.Column<long>(type: "bigint", nullable: false),
                    RAPORTAKIPNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RAPORSIRANO = table.Column<int>(type: "int", nullable: false),
                    POLIKLINIKTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YATRAPBASTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YATRAPBITTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ABASTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ABITTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ISBASKONTTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DOGUMONCBASTAR = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ISKAZASITARIHI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RAPORDURUMU = table.Column<int>(type: "int", nullable: false),
                    VAKA = table.Column<int>(type: "int", nullable: false),
                    VAKAADI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ARSIV = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_SGK_HealthReports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_SGK_HealthReports");
        }
    }
}
