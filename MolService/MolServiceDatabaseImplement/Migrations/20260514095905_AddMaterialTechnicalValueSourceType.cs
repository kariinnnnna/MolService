using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolServiceDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialTechnicalValueSourceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalKey",
                table: "material_technical_values",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SourceType",
                table: "material_technical_values",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_values_ExternalKey",
                table: "material_technical_values",
                column: "ExternalKey");

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_values_SourceType",
                table: "material_technical_values",
                column: "SourceType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_material_technical_values_ExternalKey",
                table: "material_technical_values");

            migrationBuilder.DropIndex(
                name: "IX_material_technical_values_SourceType",
                table: "material_technical_values");

            migrationBuilder.DropColumn(
                name: "ExternalKey",
                table: "material_technical_values");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "material_technical_values");
        }
    }
}
