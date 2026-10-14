using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseMediaPathsToJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.AddColumn<string>(
                name: "media_paths",
                table: "exercises",
                type: "jsonb",
                nullable: true);
            migrationBuilder.Sql(@" UPDATE exercises
set media_paths =  
case 
when media_path is null then '[]'
else jsonb_build_array(jsonb_build_object('value', media_path)) 
END ");
            migrationBuilder.DropColumn(
                name: "media_path",
                table: "exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "media_path",
                table: "exercises",
                type: "text",
                nullable: true);
            migrationBuilder.Sql(@" UPDATE exercises
set media_path = media_paths -> 0 ->> 'value'");
            migrationBuilder.DropColumn(
                name: "media_paths",
                table: "exercises");

            
        }
    }
}
