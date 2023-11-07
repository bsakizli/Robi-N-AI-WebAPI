using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class TicketWaitingHistoryUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "MainUserFullName",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "SubUserFullName",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "SubUserId",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "TicketDesc",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "WaitingReason",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.RenameColumn(
                name: "WaitingReasonLabel",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                newName: "TicketIdDesc");

            migrationBuilder.RenameColumn(
                name: "WaitingDate",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                newName: "CallBackDate");

            migrationBuilder.AlterColumn<int>(
                name: "MainUserId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaitingReasonId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.DropColumn(
                name: "WaitingReasonId",
                table: "RBN_EMPTOR_WaitingTicketHistory");

            migrationBuilder.RenameColumn(
                name: "TicketIdDesc",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                newName: "WaitingReasonLabel");

            migrationBuilder.RenameColumn(
                name: "CallBackDate",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                newName: "WaitingDate");

            migrationBuilder.AlterColumn<int>(
                name: "MainUserId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainUserFullName",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubUserFullName",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUserId",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TicketDesc",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingReason",
                table: "RBN_EMPTOR_WaitingTicketHistory",
                type: "int",
                nullable: true);
        }
    }
}
