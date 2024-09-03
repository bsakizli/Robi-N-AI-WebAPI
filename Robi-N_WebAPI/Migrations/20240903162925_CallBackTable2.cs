using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CallBackTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CallCode",
                table: "RBN_RequestACallBack",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "active",
                table: "RBN_RequestACallBack",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "add_date",
                table: "RBN_RequestACallBack",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallCode",
                table: "RBN_RequestACallBack");

            migrationBuilder.DropColumn(
                name: "active",
                table: "RBN_RequestACallBack");

            migrationBuilder.DropColumn(
                name: "add_date",
                table: "RBN_RequestACallBack");
        }
    }
}
