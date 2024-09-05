using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class IVRTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RBN_IVR_AutomaticSurveyAnswers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallId = table.Column<int>(type: "int", nullable: false),
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    QuestionID = table.Column<int>(type: "int", nullable: false),
                    AnswerKeying = table.Column<int>(type: "int", nullable: false),
                    arayan_no = table.Column<long>(type: "bigint", nullable: false),
                    aranan_no = table.Column<long>(type: "bigint", nullable: false),
                    santral_no = table.Column<long>(type: "bigint", nullable: false),
                    add_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_IVR_AutomaticSurveyAnswers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RBN_IVR_AutomaticSurveyCallNumbers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyCheck = table.Column<bool>(type: "bit", nullable: false),
                    CallId = table.Column<int>(type: "int", nullable: false),
                    number = table.Column<long>(type: "bigint", nullable: false),
                    add_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    process = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    CallDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RBN_IVR_AutomaticSurveyCallNumbers", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RBN_IVR_AutomaticSurveyAnswers");

            migrationBuilder.DropTable(
                name: "RBN_IVR_AutomaticSurveyCallNumbers");
        }
    }
}
