using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ar_Card_Connect_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "surname_name",
                table: "UserProfiles",
                newName: "surname");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "UserProfiles",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("ALTER TABLE \"UserCards\" ALTER COLUMN social_links TYPE text[] USING CASE WHEN social_links IS NULL THEN ARRAY[]::text[] ELSE ARRAY[social_links] END;");

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "UserCards",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phone",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "UserCards");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "UserProfiles",
                newName: "surname_name");

            migrationBuilder.Sql("ALTER TABLE \"UserCards\" ALTER COLUMN social_links TYPE text USING (social_links[1]);");

        }
    }
}
