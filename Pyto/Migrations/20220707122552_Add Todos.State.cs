using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pyto.Migrations
{
    public partial class AddTodosState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Todos");
        }
    }
}
