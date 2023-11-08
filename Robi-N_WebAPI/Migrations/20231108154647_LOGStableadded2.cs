using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class LOGStableadded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "uniqId",
                table: "RBN_IVR_LOGS",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RBN_IVR_LOGS_Id_uniqId",
                table: "RBN_IVR_LOGS",
                columns: new[] { "Id", "uniqId" },
                unique: true,
                filter: "[uniqId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RBN_IVR_LOGS_Id_uniqId",
                table: "RBN_IVR_LOGS");

            migrationBuilder.AlterColumn<string>(
                name: "uniqId",
                table: "RBN_IVR_LOGS",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
