using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GAMER.Data.Migrations
{
    /// <inheritdoc />
    public partial class Segunda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideojuegoPlataforma_Plataforma_PlataformaId",
                table: "VideojuegoPlataforma");

            migrationBuilder.DropForeignKey(
                name: "FK_VideojuegoPlataforma_Videojuego_VideojuegoId",
                table: "VideojuegoPlataforma");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideojuegoPlataforma",
                table: "VideojuegoPlataforma");

            migrationBuilder.RenameTable(
                name: "VideojuegoPlataforma",
                newName: "VideojuegoPlataformas");

            migrationBuilder.RenameIndex(
                name: "IX_VideojuegoPlataforma_VideojuegoId",
                table: "VideojuegoPlataformas",
                newName: "IX_VideojuegoPlataformas_VideojuegoId");

            migrationBuilder.RenameIndex(
                name: "IX_VideojuegoPlataforma_PlataformaId",
                table: "VideojuegoPlataformas",
                newName: "IX_VideojuegoPlataformas_PlataformaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideojuegoPlataformas",
                table: "VideojuegoPlataformas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideojuegoPlataformas_Plataforma_PlataformaId",
                table: "VideojuegoPlataformas",
                column: "PlataformaId",
                principalTable: "Plataforma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideojuegoPlataformas_Videojuego_VideojuegoId",
                table: "VideojuegoPlataformas",
                column: "VideojuegoId",
                principalTable: "Videojuego",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VideojuegoPlataformas_Plataforma_PlataformaId",
                table: "VideojuegoPlataformas");

            migrationBuilder.DropForeignKey(
                name: "FK_VideojuegoPlataformas_Videojuego_VideojuegoId",
                table: "VideojuegoPlataformas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VideojuegoPlataformas",
                table: "VideojuegoPlataformas");

            migrationBuilder.RenameTable(
                name: "VideojuegoPlataformas",
                newName: "VideojuegoPlataforma");

            migrationBuilder.RenameIndex(
                name: "IX_VideojuegoPlataformas_VideojuegoId",
                table: "VideojuegoPlataforma",
                newName: "IX_VideojuegoPlataforma_VideojuegoId");

            migrationBuilder.RenameIndex(
                name: "IX_VideojuegoPlataformas_PlataformaId",
                table: "VideojuegoPlataforma",
                newName: "IX_VideojuegoPlataforma_PlataformaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VideojuegoPlataforma",
                table: "VideojuegoPlataforma",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VideojuegoPlataforma_Plataforma_PlataformaId",
                table: "VideojuegoPlataforma",
                column: "PlataformaId",
                principalTable: "Plataforma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideojuegoPlataforma_Videojuego_VideojuegoId",
                table: "VideojuegoPlataforma",
                column: "VideojuegoId",
                principalTable: "Videojuego",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
