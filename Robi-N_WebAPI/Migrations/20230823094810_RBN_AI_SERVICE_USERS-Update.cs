using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RBN_AI_SERVICE_USERSUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "RBN_AI_SERVICE_USERS",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "add_date",
                table: "RBN_AI_SERVICE_USERS",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "update_date",
                table: "RBN_AI_SERVICE_USERS",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "active",
                table: "RBN_AI_SERVICE_USERS");

            migrationBuilder.DropColumn(
                name: "add_date",
                table: "RBN_AI_SERVICE_USERS");

            migrationBuilder.DropColumn(
                name: "update_date",
                table: "RBN_AI_SERVICE_USERS");
        }
    }
}
