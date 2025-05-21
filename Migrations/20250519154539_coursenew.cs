using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COURSEPROJECT.Migrations
{
    /// <inheritdoc />
    public partial class coursenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseFiles");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "CourseMaterials");

            migrationBuilder.DropColumn(
                name: "MaterialType",
                table: "CourseMaterials");

            migrationBuilder.CreateTable(
                name: "CourseFile",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseMaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseFile_CourseMaterials_CourseMaterialId",
                        column: x => x.CourseMaterialId,
                        principalTable: "CourseMaterials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseFile_CourseMaterialId",
                table: "CourseFile",
                column: "CourseMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseFile");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "CourseMaterials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseFiles_CourseMaterials_CourseMaterialId",
                        column: x => x.CourseMaterialId,
                        principalTable: "CourseMaterials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseFiles_CourseMaterialId",
                table: "CourseFiles",
                column: "CourseMaterialId");
        }
    }
}
