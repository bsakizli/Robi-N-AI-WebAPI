using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class IVRTableKeyUpdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IVR_HOLIDAY_DAYS",
                table: "IVR_HOLIDAY_DAYS");

            migrationBuilder.RenameTable(
                name: "IVR_HOLIDAY_DAYS",
                newName: "RBN_IVR_HOLIDAY_DAYS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RBN_IVR_HOLIDAY_DAYS",
                table: "RBN_IVR_HOLIDAY_DAYS",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RBN_IVR_HOLIDAY_DAYS",
                table: "RBN_IVR_HOLIDAY_DAYS");

            migrationBuilder.RenameTable(
                name: "RBN_IVR_HOLIDAY_DAYS",
                newName: "IVR_HOLIDAY_DAYS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IVR_HOLIDAY_DAYS",
                table: "IVR_HOLIDAY_DAYS",
                column: "Id");
        }
    }
}
