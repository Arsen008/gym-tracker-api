using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_SoftDelete_To_Exercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "_isDeleted",
                table: "exercises",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_isDeleted",
                table: "exercises");
        }
    }
}
