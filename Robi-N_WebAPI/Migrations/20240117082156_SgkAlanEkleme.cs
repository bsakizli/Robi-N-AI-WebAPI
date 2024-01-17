using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SgkAlanEkleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "RBN_SGK_HealthReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "addDate",
                table: "RBN_SGK_HealthReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "mailSend",
                table: "RBN_SGK_HealthReports",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "addDate",
                table: "RBN_SGK_HealthReports");

            migrationBuilder.DropColumn(
                name: "mailSend",
                table: "RBN_SGK_HealthReports");
        }
    }
}
