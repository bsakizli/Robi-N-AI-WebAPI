using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RBN_AI_SERVICE_USERSUpdate7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_date",
                table: "RBN_AI_SERVICE_ROLE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "add_date",
                table: "RBN_AI_SERVICE_ROLE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "RBN_AI_SERVICE_ROLE",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RBN_AI_SERVICE_ROLES_MAP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    add_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    update_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_AI_SERVICE_ROLES_MAP", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_AI_SERVICE_ROLES_MAP");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "RBN_AI_SERVICE_ROLE");

            migrationBuilder.AlterColumn<DateTime>(
                name: "update_date",
                table: "RBN_AI_SERVICE_ROLE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "add_date",
                table: "RBN_AI_SERVICE_ROLE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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
    }
}
