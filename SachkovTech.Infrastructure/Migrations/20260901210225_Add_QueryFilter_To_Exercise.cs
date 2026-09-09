using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_QueryFilter_To_Exercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_isDeleted",
                table: "exercises",
                newName: "is_Deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_Deleted",
                table: "exercises",
                newName: "_isDeleted");
        }
    }
}
