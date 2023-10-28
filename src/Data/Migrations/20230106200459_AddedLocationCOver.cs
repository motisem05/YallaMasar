using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaMasar.Migrations
{
    public partial class AddedLocationCOver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Trips",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "Trips",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Trips",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "LocationsInfo",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "LocationsInfo",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "LocationsInfo",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Locations",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "Locations",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Locations",
                newName: "CreatedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateDate",
                table: "Images",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "Images",
                newName: "ModifiedBy");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Images",
                newName: "CreatedOn");

            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Trips",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Trips",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Trips",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "LocationsInfo",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "LocationsInfo",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "LocationsInfo",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Locations",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Locations",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Locations",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Images",
                newName: "UpdateDate");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "Images",
                newName: "UpdateBy");

            migrationBuilder.RenameColumn(
                name: "CreatedOn",
                table: "Images",
                newName: "CreateDate");
        }
    }
}
