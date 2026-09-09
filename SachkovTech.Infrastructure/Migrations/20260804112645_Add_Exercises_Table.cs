using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SachkovTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Exercises_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ToDoItems",
                table: "ToDoItems");

            migrationBuilder.RenameTable(
                name: "ToDoItems",
                newName: "todo_items");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "todo_items",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_todo_items",
                table: "todo_items",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    MuscleGroup = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exercises", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_todo_items",
                table: "todo_items");

            migrationBuilder.RenameTable(
                name: "todo_items",
                newName: "ToDoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ToDoItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ToDoItems",
                table: "ToDoItems",
                column: "Id");
        }
    }
}
