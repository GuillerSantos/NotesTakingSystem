using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote");

            migrationBuilder.CreateTable(
                name: "SharedNotes",
                columns: table => new
                {
                    SharedNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedNotes", x => x.SharedNoteId);
                    table.ForeignKey(
                        name: "FK_SharedNotes_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "SharedNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "SharedNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote");

            migrationBuilder.DropTable(
                name: "SharedNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNote_Notes_NoteId",
                table: "FavoriteNote",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
