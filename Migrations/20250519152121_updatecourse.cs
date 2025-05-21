using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COURSEPROJECT.Migrations
{
    public partial class updatecourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LiveStreamUrl",
                table: "CourseMaterials",
                type: "nvarchar(max)",
                nullable: true);  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiveStreamUrl",
                table: "CourseMaterials");
        }
    }
}
