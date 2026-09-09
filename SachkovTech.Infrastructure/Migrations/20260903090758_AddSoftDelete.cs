using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_Deleted",
                table: "exercises",
                newName: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "exercises",
                newName: "is_Deleted");
        }
    }
}
