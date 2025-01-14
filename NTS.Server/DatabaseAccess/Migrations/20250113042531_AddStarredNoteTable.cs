using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddStarredNoteTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNote_ApplicationUsers_UserId",
                table: "FavoriteNote");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteNote",
                table: "FavoriteNote");

            migrationBuilder.RenameTable(
                name: "FavoriteNote",
                newName: "FavoriteNotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteNotes",
                table: "FavoriteNotes",
                column: "FavoriteNoteId");

            migrationBuilder.CreateTable(
                name: "StarredNotes",
                columns: table => new
                {
                    StarredNotesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StarredNotes", x => x.StarredNotesId);
                    table.ForeignKey(
                        name: "FK_StarredNotes_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_StarredNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "StarredNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "StarredNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNotes_ApplicationUsers_UserId",
                table: "FavoriteNotes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNotes_ApplicationUsers_UserId",
                table: "FavoriteNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes");

            migrationBuilder.DropTable(
                name: "StarredNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteNotes",
                table: "FavoriteNotes");

            migrationBuilder.RenameTable(
                name: "FavoriteNotes",
                newName: "FavoriteNote");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteNote",
                table: "FavoriteNote",
                column: "FavoriteNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNote_ApplicationUsers_UserId",
                table: "FavoriteNote",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");
        }
    }
}
