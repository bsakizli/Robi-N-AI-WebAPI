using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RBN_AI_SERVICE_USERSUpdate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "RBN_AI_SERVICE_ROLE",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RBN_AI_SERVICE_ROLE",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "RBN_AI_SERVICE_ROLE",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
