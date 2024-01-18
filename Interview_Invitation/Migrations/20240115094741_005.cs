using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interview_Invitation.Migrations
{
    public partial class _005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvFilePath",
                table: "Interviewees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvFilePath",
                table: "Interviewees");
        }
    }
}
