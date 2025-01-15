using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class ResetConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportantNotes_ApplicationUsers_NoteId",
                table: "ImportantNotes");

            migrationBuilder.CreateIndex(
                name: "IX_ImportantNotes_UserId",
                table: "ImportantNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantNotes_ApplicationUsers_UserId",
                table: "ImportantNotes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportantNotes_ApplicationUsers_UserId",
                table: "ImportantNotes");

            migrationBuilder.DropIndex(
                name: "IX_ImportantNotes_UserId",
                table: "ImportantNotes");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportantNotes_ApplicationUsers_NoteId",
                table: "ImportantNotes",
                column: "NoteId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
