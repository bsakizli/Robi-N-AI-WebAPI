using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class TicketWaitingHistoryAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RBN_EMPTOR_WaitingTicketHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    TicketDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainUserId = table.Column<int>(type: "int", nullable: true),
                    MainUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubUserId = table.Column<int>(type: "int", nullable: true),
                    SubUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaitingReason = table.Column<int>(type: "int", nullable: true),
                    WaitingReasonLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaitingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_EMPTOR_WaitingTicketHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_EMPTOR_WaitingTicketHistory");
        }
    }
}
