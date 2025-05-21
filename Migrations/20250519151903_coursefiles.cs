using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COURSEPROJECT.Migrations
{
    /// <inheritdoc />
    public partial class coursefiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LiveStartTime",
                table: "CourseMaterials",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialType",
                table: "CourseMaterials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CourseFiles",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseMaterialId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseFiles_CourseMaterials_CourseMaterialId",
                        column: x => x.CourseMaterialId,
                        principalTable: "CourseMaterials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseFiles_CourseMaterialId",
                table: "CourseFiles",
                column: "CourseMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseFiles");

            migrationBuilder.DropColumn(
                name: "LiveStartTime",
                table: "CourseMaterials");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "CourseMaterials");
        }
    }
}
