using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication_Ejemplo_1.Migrations;

/// <inheritdoc />
public partial class _20261124162729_InicialTesis : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Tutores",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Especialidad = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                Correo = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tutores", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Usuarios",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                NombreUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Rol = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Usuarios", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Tesis",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Titulo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false, collation: "Latin1_General_100_CI_AS"),
                Estudiante = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Imagen = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                IdTutor = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tesis", x => x.Id);
                table.ForeignKey(
                    name: "FK_Tesis_Tutores_IdTutor",
                    column: x => x.IdTutor,
                    principalTable: "Tutores",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Tesis_IdTutor",
            table: "Tesis",
            column: "IdTutor");

        migrationBuilder.CreateIndex(
            name: "IX_Tesis_Titulo",
            table: "Tesis",
            column: "Titulo",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Tutores_Correo",
            table: "Tutores",
            column: "Correo",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Usuarios_NombreUsuario",
            table: "Usuarios",
            column: "NombreUsuario",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Tesis");

        migrationBuilder.DropTable(
            name: "Usuarios");

        migrationBuilder.DropTable(
            name: "Tutores");
    }
}
