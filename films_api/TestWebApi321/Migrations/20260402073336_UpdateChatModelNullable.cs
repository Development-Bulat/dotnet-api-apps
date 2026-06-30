using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWebApi321.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatModelNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Films_id_Film",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_id_Recepient",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_id_Recepient",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "id_Recepient",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "id_Film",
                table: "ChatMessages",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "id_Recipient",
                table: "ChatMessages",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_id_Recipient",
                table: "ChatMessages",
                column: "id_Recipient");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Films_id_Film",
                table: "ChatMessages",
                column: "id_Film",
                principalTable: "Films",
                principalColumn: "id_Film");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_id_Recipient",
                table: "ChatMessages",
                column: "id_Recipient",
                principalTable: "Users",
                principalColumn: "id_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Films_id_Film",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_id_Recipient",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_id_Recipient",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "id_Recipient",
                table: "ChatMessages");

            migrationBuilder.AlterColumn<int>(
                name: "id_Film",
                table: "ChatMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_Recepient",
                table: "ChatMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_id_Recepient",
                table: "ChatMessages",
                column: "id_Recepient");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Films_id_Film",
                table: "ChatMessages",
                column: "id_Film",
                principalTable: "Films",
                principalColumn: "id_Film",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_id_Recepient",
                table: "ChatMessages",
                column: "id_Recepient",
                principalTable: "Users",
                principalColumn: "id_User",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
