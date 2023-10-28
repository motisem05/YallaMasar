using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaMasar.Migrations
{
    public partial class AddedInterestingPlaceCover : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "InterestingPlaces",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "InterestingPlaces");
        }
    }
}
