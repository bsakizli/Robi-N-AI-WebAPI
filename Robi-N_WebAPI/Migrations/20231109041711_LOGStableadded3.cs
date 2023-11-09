using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class LOGStableadded3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "logKey",
                table: "RBN_IVR_LOGS",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "logKey",
                table: "RBN_IVR_LOGS");
        }
    }
}
