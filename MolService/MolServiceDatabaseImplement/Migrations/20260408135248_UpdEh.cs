using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolServiceDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class UpdEh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "equipment_movement_histories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "equipment_movement_histories",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "equipment_movement_histories");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "equipment_movement_histories");
        }
    }
}
