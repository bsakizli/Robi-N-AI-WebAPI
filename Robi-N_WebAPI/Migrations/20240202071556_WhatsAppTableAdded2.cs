using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Robi_N_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class WhatsAppTableAdded2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageBody",
                table: "RBN_WhatsAppMessageTemplate");

            migrationBuilder.AddColumn<int>(
                name: "SmsId",
                table: "RBN_WhatsAppMessageTemplate",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmsId",
                table: "RBN_WhatsAppMessageTemplate");

            migrationBuilder.AddColumn<string>(
                name: "MessageBody",
                table: "RBN_WhatsAppMessageTemplate",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
