using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AutoMaticTicketClosedTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RBN_EMPTOR_AUTOTICKETCLOSEDScheduler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    process = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    oneClosedTicketCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ticketQuery = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    registerDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_EMPTOR_AUTOTICKETCLOSEDScheduler", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_EMPTOR_AUTOTICKETCLOSEDScheduler");
        }
    }
}
