using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ar_Card_Connect_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateToUserCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "template",
                table: "UserCards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "template",
                table: "UserCards");
        }
    }
}
