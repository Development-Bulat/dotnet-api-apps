using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestWebApi321.Migrations
{
    /// <inheritdoc />
    public partial class FinalTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    id_Message = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    id_Sender = table.Column<int>(type: "integer", nullable: false),
                    id_Film = table.Column<int>(type: "integer", nullable: false),
                    id_Recepient = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.id_Message);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Films_id_Film",
                        column: x => x.id_Film,
                        principalTable: "Films",
                        principalColumn: "id_Film",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_id_Recepient",
                        column: x => x.id_Recepient,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Users_id_Sender",
                        column: x => x.id_Sender,
                        principalTable: "Users",
                        principalColumn: "id_User",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_id_Film",
                table: "ChatMessages",
                column: "id_Film");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_id_Recepient",
                table: "ChatMessages",
                column: "id_Recepient");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_id_Sender",
                table: "ChatMessages",
                column: "id_Sender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");
        }
    }
}
