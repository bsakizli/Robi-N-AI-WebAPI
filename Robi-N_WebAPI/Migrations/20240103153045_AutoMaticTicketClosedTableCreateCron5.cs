using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AutoMaticTicketClosedTableCreateCron5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "oneClosedTicketCount",
                table: "RBN_EMPTOR_AUTOTICKETCLOSEDScheduler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "oneClosedTicketCount",
                table: "RBN_EMPTOR_AUTOTICKETCLOSEDScheduler",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
