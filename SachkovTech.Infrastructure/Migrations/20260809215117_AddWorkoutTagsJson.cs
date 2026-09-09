using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutTagsJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "tags",
                table: "workouts",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tags",
                table: "workouts");
        }
    }
}
