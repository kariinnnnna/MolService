using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MolServiceDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class FixEquipmentMovementHistoryMoveDateType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MoveDate",
                table: "equipment_movement_histories",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MoveDate",
                table: "equipment_movement_histories",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
