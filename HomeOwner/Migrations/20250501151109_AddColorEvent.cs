using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeOwner.Migrations
{
    /// <inheritdoc />
    public partial class AddColorEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackgroundColor",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "Events",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundColor",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "Events");
        }
    }
}
