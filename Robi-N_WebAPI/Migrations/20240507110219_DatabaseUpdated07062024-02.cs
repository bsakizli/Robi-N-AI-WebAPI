using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdated0706202402 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "calltype",
                table: "RBN_UnansweredCalls",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "agentid",
                table: "RBN_UnansweredCalls",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MessageCode",
                table: "RBN_SMS_TEMPLATES",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageCode",
                table: "RBN_SMS_TEMPLATES");

            migrationBuilder.AlterColumn<int>(
                name: "calltype",
                table: "RBN_UnansweredCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "agentid",
                table: "RBN_UnansweredCalls",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
