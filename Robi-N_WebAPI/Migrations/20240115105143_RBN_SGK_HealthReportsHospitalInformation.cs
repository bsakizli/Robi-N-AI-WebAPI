using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RBN_SGK_HealthReportsHospitalInformation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BASHEKIMONAYTARIHI",
                table: "RBN_SGK_HealthReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ISVERENEBILDIRILDIGITARIH",
                table: "RBN_SGK_HealthReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RAPORBITTAR",
                table: "RBN_SGK_HealthReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TESISADI",
                table: "RBN_SGK_HealthReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TESISKODU",
                table: "RBN_SGK_HealthReports",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BASHEKIMONAYTARIHI",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "ISVERENEBILDIRILDIGITARIH",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "RAPORBITTAR",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "TESISADI",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "TESISKODU",
                table: "RBN_SGK_HealthReports");
        }
    }
}
