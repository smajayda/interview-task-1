using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Interview_Invitation.Migrations
{
    public partial class _002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvFileName",
                table: "Interviewees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvFileName",
                table: "Interviewees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
