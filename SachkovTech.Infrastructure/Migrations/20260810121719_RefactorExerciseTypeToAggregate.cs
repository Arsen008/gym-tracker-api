using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorExerciseTypeToAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             
            migrationBuilder.Sql("TRUNCATE TABLE workout_exercises, exercises CASCADE;");
            
            migrationBuilder.RenameColumn(
                name: "ExerciseId",
                table: "workout_exercises",
                newName: "exercise_id");

            migrationBuilder.AddColumn<Guid>(
                name: "exercise_type_id",
                table: "workout_exercises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExerciseTypeId",
                table: "exercises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "exercise_types",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercise_types", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exercises_ExerciseTypeId",
                table: "exercises",
                column: "ExerciseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_exercises_exercise_types_ExerciseTypeId",
                table: "exercises",
                column: "ExerciseTypeId",
                principalTable: "exercise_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exercises_exercise_types_ExerciseTypeId",
                table: "exercises");

            migrationBuilder.DropTable(
                name: "exercise_types");

            migrationBuilder.DropIndex(
                name: "IX_exercises_ExerciseTypeId",
                table: "exercises");

            migrationBuilder.DropColumn(
                name: "exercise_type_id",
                table: "workout_exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeId",
                table: "exercises");

            migrationBuilder.RenameColumn(
                name: "exercise_id",
                table: "workout_exercises",
                newName: "ExerciseId");
        }
    }
}
