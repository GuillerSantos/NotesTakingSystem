using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NTS.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsPublicProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Notes",
                type: "bit",
                maxLength: 255,
                nullable: false,
                defaultValue: false);
        }
    }
}
