using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class SGKTableGuncelleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hash",
                table: "RBN_SGK_VisitingIntroductionInformation");

            migrationBuilder.DropColumn(
                name: "username",
                table: "RBN_SGK_VisitingIntroductionInformation");

            migrationBuilder.DropColumn(
                name: "workplaceCode",
                table: "RBN_SGK_VisitingIntroductionInformation");

            migrationBuilder.RenameColumn(
                name: "workplacePassword",
                table: "RBN_SGK_VisitingIntroductionInformation",
                newName: "value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "value",
                table: "RBN_SGK_VisitingIntroductionInformation",
                newName: "workplacePassword");

            migrationBuilder.AddColumn<string>(
                name: "hash",
                table: "RBN_SGK_VisitingIntroductionInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "RBN_SGK_VisitingIntroductionInformation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workplaceCode",
                table: "RBN_SGK_VisitingIntroductionInformation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
