using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddImportantNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportantNotes",
                columns: table => new
                {
                    ImportantNoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportantNotes", x => x.ImportantNoteId);
                    table.ForeignKey(
                        name: "FK_ImportantNotes_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportantNotes_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "ImportantNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "ImportantNotes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportantNotes");
        }
    }
}
