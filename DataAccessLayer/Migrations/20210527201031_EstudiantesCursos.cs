using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class EstudiantesCursos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "AspNetUsers",
                type: "LONGTEXT",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EstudiantesCursos",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CursosId = table.Column<long>(type: "bigint", nullable: false),
                    EstudiantesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantesCursos", x => x.id);
                    table.ForeignKey(
                        name: "FK_EstudiantesCursos_Cursos_CursosId",
                        column: x => x.CursosId,
                        principalTable: "Cursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudiantesCursos_Estudiantes_EstudiantesId",
                        column: x => x.EstudiantesId,
                        principalTable: "Estudiantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesCursos_CursosId",
                table: "EstudiantesCursos",
                column: "CursosId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesCursos_EstudiantesId",
                table: "EstudiantesCursos",
                column: "EstudiantesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudiantesCursos");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");
        }
    }
}
