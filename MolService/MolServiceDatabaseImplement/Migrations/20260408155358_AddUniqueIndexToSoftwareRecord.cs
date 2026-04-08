using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolServiceDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToSoftwareRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_software_records_MaterialTechnicalValueId",
                table: "software_records");

            migrationBuilder.CreateIndex(
                name: "IX_software_records_MaterialTechnicalValueId_SoftwareId",
                table: "software_records",
                columns: new[] { "MaterialTechnicalValueId", "SoftwareId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_software_records_MaterialTechnicalValueId_SoftwareId",
                table: "software_records");

            migrationBuilder.CreateIndex(
                name: "IX_software_records_MaterialTechnicalValueId",
                table: "software_records",
                column: "MaterialTechnicalValueId");
        }
    }
}
