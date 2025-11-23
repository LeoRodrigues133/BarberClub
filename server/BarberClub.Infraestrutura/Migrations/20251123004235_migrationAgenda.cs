using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberClub.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class migrationAgenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBCONFIGURACAO_AspNetUsers_UsuarioId",
                table: "TBCONFIGURACAO");

            migrationBuilder.DropForeignKey(
                name: "FK_TBHORARIOFUNCIONAMENTO_TBCONFIGURACAO_ConfiguracaoEmpresaId",
                table: "TBHORARIOFUNCIONAMENTO");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TBCONFIGURACAO",
                table: "TBCONFIGURACAO");

            migrationBuilder.RenameTable(
                name: "TBCONFIGURACAO",
                newName: "ConfiguracaoEmpresa");

            migrationBuilder.RenameIndex(
                name: "IX_TBCONFIGURACAO_UsuarioId",
                table: "ConfiguracaoEmpresa",
                newName: "IX_ConfiguracaoEmpresa_UsuarioId");

            migrationBuilder.AlterColumn<string>(
                name: "NomeEmpresa",
                table: "ConfiguracaoEmpresa",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "ConfiguracaoEmpresa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BannerUrl",
                table: "ConfiguracaoEmpresa",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ConfiguracaoEmpresa",
                table: "ConfiguracaoEmpresa",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "TBCONFIGURACAOAGENDA",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FuncionarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TempoAtendimento = table.Column<int>(type: "int", nullable: false),
                    IntervaloEntreAtendimento = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCONFIGURACAOAGENDA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCONFIGURACAOAGENDA_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TBCONFIGURACAOAGENDA_TBFUNCIONARIO_FuncionarioId",
                        column: x => x.FuncionarioId,
                        principalTable: "TBFUNCIONARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBCONFIGURACAOAGENDA_FuncionarioId",
                table: "TBCONFIGURACAOAGENDA",
                column: "FuncionarioId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCONFIGURACAOAGENDA_UsuarioId",
                table: "TBCONFIGURACAOAGENDA",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConfiguracaoEmpresa_AspNetUsers_UsuarioId",
                table: "ConfiguracaoEmpresa",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TBHORARIOFUNCIONAMENTO_ConfiguracaoEmpresa_ConfiguracaoEmpresaId",
                table: "TBHORARIOFUNCIONAMENTO",
                column: "ConfiguracaoEmpresaId",
                principalTable: "ConfiguracaoEmpresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConfiguracaoEmpresa_AspNetUsers_UsuarioId",
                table: "ConfiguracaoEmpresa");

            migrationBuilder.DropForeignKey(
                name: "FK_TBHORARIOFUNCIONAMENTO_ConfiguracaoEmpresa_ConfiguracaoEmpresaId",
                table: "TBHORARIOFUNCIONAMENTO");

            migrationBuilder.DropTable(
                name: "TBCONFIGURACAOAGENDA");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ConfiguracaoEmpresa",
                table: "ConfiguracaoEmpresa");

            migrationBuilder.RenameTable(
                name: "ConfiguracaoEmpresa",
                newName: "TBCONFIGURACAO");

            migrationBuilder.RenameIndex(
                name: "IX_ConfiguracaoEmpresa_UsuarioId",
                table: "TBCONFIGURACAO",
                newName: "IX_TBCONFIGURACAO_UsuarioId");

            migrationBuilder.AlterColumn<string>(
                name: "NomeEmpresa",
                table: "TBCONFIGURACAO",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUrl",
                table: "TBCONFIGURACAO",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BannerUrl",
                table: "TBCONFIGURACAO",
                type: "varchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TBCONFIGURACAO",
                table: "TBCONFIGURACAO",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TBCONFIGURACAO_AspNetUsers_UsuarioId",
                table: "TBCONFIGURACAO",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TBHORARIOFUNCIONAMENTO_TBCONFIGURACAO_ConfiguracaoEmpresaId",
                table: "TBHORARIOFUNCIONAMENTO",
                column: "ConfiguracaoEmpresaId",
                principalTable: "TBCONFIGURACAO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
