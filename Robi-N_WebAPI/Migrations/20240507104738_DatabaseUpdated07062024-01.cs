using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseUpdated0706202401 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RBN_UnansweredCalls",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contactid = table.Column<long>(type: "bigint", nullable: false),
                    agentid = table.Column<int>(type: "int", nullable: false),
                    calltype = table.Column<int>(type: "int", nullable: false),
                    phonenumber = table.Column<long>(type: "bigint", nullable: false),
                    disposition = table.Column<int>(type: "int", nullable: false),
                    csqname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startdatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    enddatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    record_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    smsSendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    smsSendStatus = table.Column<bool>(type: "bit", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_UnansweredCalls", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_UnansweredCalls");
        }
    }
}
