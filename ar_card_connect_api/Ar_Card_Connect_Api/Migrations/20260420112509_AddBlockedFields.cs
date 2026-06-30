using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ar_Card_Connect_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "UserProfiles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "UserCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "UserCards");
        }
    }
}
