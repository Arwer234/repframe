using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepFrame.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDateTimeOffsetToDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "WorkoutSessions",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "WorkoutSessions");
        }
    }
}
