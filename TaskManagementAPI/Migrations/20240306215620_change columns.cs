using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class changecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProjectTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "ProjectTasks",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
