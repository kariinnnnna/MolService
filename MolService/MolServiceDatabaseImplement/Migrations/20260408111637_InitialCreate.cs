using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MolServiceDatabaseImplement.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CoreSystemId = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    NotUseInSchedule = table.Column<bool>(type: "boolean", nullable: false),
                    HasProjector = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_responsible_persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_responsible_persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_technical_value_groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_technical_value_groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "softwares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SoftwareName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SoftwareDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SoftwareKey = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    SoftwareK = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_softwares", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "material_technical_values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ClassroomId = table.Column<int>(type: "integer", nullable: true),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Quantity = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cost = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaterialResponsiblePersonId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_technical_values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_material_technical_values_classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_technical_values_material_responsible_persons_Mate~",
                        column: x => x.MaterialResponsiblePersonId,
                        principalTable: "material_responsible_persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "equipment_movement_histories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MoveDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MaterialTechnicalValueId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipment_movement_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_equipment_movement_histories_material_technical_values_Mate~",
                        column: x => x.MaterialTechnicalValueId,
                        principalTable: "material_technical_values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_technical_value_records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialTechnicalValueGroupId = table.Column<int>(type: "integer", nullable: false),
                    MaterialTechnicalValueId = table.Column<int>(type: "integer", nullable: false),
                    FieldName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    FieldValue = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_technical_value_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_material_technical_value_records_material_technical_value_g~",
                        column: x => x.MaterialTechnicalValueGroupId,
                        principalTable: "material_technical_value_groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_technical_value_records_material_technical_values_~",
                        column: x => x.MaterialTechnicalValueId,
                        principalTable: "material_technical_values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "software_records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MaterialTechnicalValueId = table.Column<int>(type: "integer", nullable: false),
                    SoftwareId = table.Column<int>(type: "integer", nullable: false),
                    SetupDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ClaimNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_software_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_software_records_material_technical_values_MaterialTechnica~",
                        column: x => x.MaterialTechnicalValueId,
                        principalTable: "material_technical_values",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_software_records_softwares_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "softwares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_classrooms_CoreSystemId",
                table: "classrooms",
                column: "CoreSystemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_classrooms_Number",
                table: "classrooms",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_equipment_movement_histories_MaterialTechnicalValueId",
                table: "equipment_movement_histories",
                column: "MaterialTechnicalValueId");

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_value_records_MaterialTechnicalValueGrou~",
                table: "material_technical_value_records",
                column: "MaterialTechnicalValueGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_value_records_MaterialTechnicalValueId",
                table: "material_technical_value_records",
                column: "MaterialTechnicalValueId");

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_values_ClassroomId",
                table: "material_technical_values",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_material_technical_values_MaterialResponsiblePersonId",
                table: "material_technical_values",
                column: "MaterialResponsiblePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_software_records_MaterialTechnicalValueId",
                table: "software_records",
                column: "MaterialTechnicalValueId");

            migrationBuilder.CreateIndex(
                name: "IX_software_records_SoftwareId",
                table: "software_records",
                column: "SoftwareId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "equipment_movement_histories");

            migrationBuilder.DropTable(
                name: "material_technical_value_records");

            migrationBuilder.DropTable(
                name: "software_records");

            migrationBuilder.DropTable(
                name: "material_technical_value_groups");

            migrationBuilder.DropTable(
                name: "material_technical_values");

            migrationBuilder.DropTable(
                name: "softwares");

            migrationBuilder.DropTable(
                name: "classrooms");

            migrationBuilder.DropTable(
                name: "material_responsible_persons");
        }
    }
}
