using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreateCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedNotes_Notes_NoteId",
                table: "SharedNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StarredNotes_ApplicationUsers_UserId",
                table: "StarredNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StarredNotes_Notes_NoteId",
                table: "StarredNotes");

            migrationBuilder.DropColumn(
                name: "FavoriteNote",
                table: "Notes");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "StarredNotes",
                newName: "IX_StarredNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "StarredNotes",
                newName: "IX_StarredNotes_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "SharedNotes",
                newName: "IX_SharedNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "SharedNotes",
                newName: "IX_SharedNotes_NoteId");

            migrationBuilder.RenameColumn(
                name: "Priority",
                table: "Notes",
                newName: "Color");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_UserId",
                table: "ImportantNotes",
                newName: "IX_ImportantNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FavoriteNotes_NoteId",
                table: "ImportantNotes",
                newName: "IX_ImportantNotes_NoteId");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "StarredNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "StarredNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "StarredNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "StarredNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "SharedNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "SharedNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "SharedNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SharedNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notes",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "ImportantNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "ImportantNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "ImportantNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ImportantNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "FavoriteNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "FavoriteNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "FavoriteNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FavoriteNotes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecoveryEmail",
                table: "ApplicationUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "ApplicationUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "ApplicationUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "ApplicationUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateJoined",
                table: "ApplicationUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommentContent = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedNotes_Notes_NoteId",
                table: "SharedNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StarredNotes_ApplicationUsers_UserId",
                table: "StarredNotes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StarredNotes_Notes_NoteId",
                table: "StarredNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedNotes_Notes_NoteId",
                table: "SharedNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StarredNotes_ApplicationUsers_UserId",
                table: "StarredNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_StarredNotes_Notes_NoteId",
                table: "StarredNotes");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "StarredNotes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "StarredNotes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "StarredNotes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "StarredNotes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "SharedNotes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "SharedNotes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "SharedNotes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SharedNotes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "ImportantNotes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "ImportantNotes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "ImportantNotes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "ImportantNotes");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "FavoriteNotes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "FavoriteNotes");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "FavoriteNotes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "FavoriteNotes");

            migrationBuilder.RenameIndex(
                name: "IX_StarredNotes_UserId",
                table: "StarredNotes",
                newName: "IX_FavoriteNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StarredNotes_NoteId",
                table: "StarredNotes",
                newName: "IX_FavoriteNotes_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_SharedNotes_UserId",
                table: "SharedNotes",
                newName: "IX_FavoriteNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SharedNotes_NoteId",
                table: "SharedNotes",
                newName: "IX_FavoriteNotes_NoteId");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Notes",
                newName: "Priority");

            migrationBuilder.RenameIndex(
                name: "IX_ImportantNotes_UserId",
                table: "ImportantNotes",
                newName: "IX_FavoriteNotes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ImportantNotes_NoteId",
                table: "ImportantNotes",
                newName: "IX_FavoriteNotes_NoteId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Notes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Notes",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<bool>(
                name: "FavoriteNote",
                table: "Notes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "ApplicationUsers",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RecoveryEmail",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "ApplicationUsers",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "ApplicationUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateJoined",
                table: "ApplicationUsers",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteNotes_Notes_NoteId",
                table: "FavoriteNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedNotes_Notes_NoteId",
                table: "SharedNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_StarredNotes_ApplicationUsers_UserId",
                table: "StarredNotes",
                column: "UserId",
                principalTable: "ApplicationUsers",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StarredNotes_Notes_NoteId",
                table: "StarredNotes",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "NoteId");
        }
    }
}
