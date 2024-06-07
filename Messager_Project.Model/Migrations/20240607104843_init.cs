using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messager_Project.Model.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_emotes",
                columns: table => new
                {
                    Emote_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Emote_Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Emote_Unicode = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: false),
                    Emote_Default_Color = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__emotes", x => x.Emote_ID);
                });

            migrationBuilder.CreateTable(
                name: "_users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    User_Picture = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "_messages",
                columns: table => new
                {
                    Message_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message_Content = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: false),
                    Sender_ID = table.Column<int>(type: "int", nullable: false),
                    Reciver_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__messages", x => x.Message_ID);
                    table.ForeignKey(
                        name: "FK__messages__users_Reciver_ID",
                        column: x => x.Reciver_ID,
                        principalTable: "_users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__messages__users_Sender_ID",
                        column: x => x.Sender_ID,
                        principalTable: "_users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "_usersFriends",
                columns: table => new
                {
                    Relation_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    User1_ID = table.Column<int>(type: "int", nullable: false),
                    User2_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__usersFriends", x => x.Relation_ID);
                    table.ForeignKey(
                        name: "FK__usersFriends__users_User1_ID",
                        column: x => x.User1_ID,
                        principalTable: "_users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__usersFriends__users_User2_ID",
                        column: x => x.User2_ID,
                        principalTable: "_users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "_messageEmotes",
                columns: table => new
                {
                    Relation_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_ID = table.Column<int>(type: "int", nullable: false),
                    Emote_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__messageEmotes", x => x.Relation_ID);
                    table.ForeignKey(
                        name: "FK__messageEmotes__emotes_Emote_ID",
                        column: x => x.Emote_ID,
                        principalTable: "_emotes",
                        principalColumn: "Emote_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__messageEmotes__messages_Message_ID",
                        column: x => x.Message_ID,
                        principalTable: "_messages",
                        principalColumn: "Message_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__messageEmotes_Emote_ID",
                table: "_messageEmotes",
                column: "Emote_ID");

            migrationBuilder.CreateIndex(
                name: "IX__messageEmotes_Message_ID",
                table: "_messageEmotes",
                column: "Message_ID");

            migrationBuilder.CreateIndex(
                name: "IX__messages_Reciver_ID",
                table: "_messages",
                column: "Reciver_ID");

            migrationBuilder.CreateIndex(
                name: "IX__messages_Sender_ID",
                table: "_messages",
                column: "Sender_ID");

            migrationBuilder.CreateIndex(
                name: "IX__usersFriends_User1_ID",
                table: "_usersFriends",
                column: "User1_ID");

            migrationBuilder.CreateIndex(
                name: "IX__usersFriends_User2_ID",
                table: "_usersFriends",
                column: "User2_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_messageEmotes");

            migrationBuilder.DropTable(
                name: "_usersFriends");

            migrationBuilder.DropTable(
                name: "_emotes");

            migrationBuilder.DropTable(
                name: "_messages");

            migrationBuilder.DropTable(
                name: "_users");
        }
    }
}
