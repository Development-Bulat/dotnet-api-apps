using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ar_Card_Connect_Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCards_UserLogins_user_id",
                table: "UserCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins");

            migrationBuilder.AddColumn<Guid>(
                name: "login_id",
                table: "UserLogins",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "UserCards",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins",
                column: "login_id");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    surname_name = table.Column<string>(type: "text", nullable: true),
                    avatar_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Roles_role_id",
                        column: x => x.role_id,
                        principalTable: "Roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_user_id",
                table: "UserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_role_id",
                table: "UserProfiles",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCards_UserProfiles_user_id",
                table: "UserCards",
                column: "user_id",
                principalTable: "UserProfiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_UserProfiles_user_id",
                table: "UserLogins",
                column: "user_id",
                principalTable: "UserProfiles",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCards_UserProfiles_user_id",
                table: "UserCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_UserProfiles_user_id",
                table: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins");

            migrationBuilder.DropIndex(
                name: "IX_UserLogins_user_id",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "login_id",
                table: "UserLogins");

            migrationBuilder.DropColumn(
                name: "status",
                table: "UserCards");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCards_UserLogins_user_id",
                table: "UserCards",
                column: "user_id",
                principalTable: "UserLogins",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
