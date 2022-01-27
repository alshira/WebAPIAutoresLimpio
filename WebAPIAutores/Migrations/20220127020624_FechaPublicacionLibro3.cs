using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIAutores.Migrations
{
    public partial class FechaPublicacionLibro3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaPublicacionLibro",
                table: "Libros",
                newName: "FechaPublicacion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FechaPublicacion",
                table: "Libros",
                newName: "FechaPublicacionLibro");
        }
    }
}
