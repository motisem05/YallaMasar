using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaMasar.Migrations
{
    public partial class AddedInterestingPlacesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Trips_TripId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_LocationId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_TripId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "InterestingPlaceEntityId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationEntityId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TripEntityId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InterestingPlaces",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestingPlaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestingPlacesInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestingPlaceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestingPlacesInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestingPlacesInfo_InterestingPlaces_InterestingPlaceId",
                        column: x => x.InterestingPlaceId,
                        principalTable: "InterestingPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_InterestingPlaceEntityId",
                table: "Images",
                column: "InterestingPlaceEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_LocationEntityId",
                table: "Images",
                column: "LocationEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TripEntityId",
                table: "Images",
                column: "TripEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestingPlacesInfo_InterestingPlaceId",
                table: "InterestingPlacesInfo",
                column: "InterestingPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_InterestingPlaces_InterestingPlaceEntityId",
                table: "Images",
                column: "InterestingPlaceEntityId",
                principalTable: "InterestingPlaces",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Locations_LocationEntityId",
                table: "Images",
                column: "LocationEntityId",
                principalTable: "Locations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Trips_TripEntityId",
                table: "Images",
                column: "TripEntityId",
                principalTable: "Trips",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_InterestingPlaces_InterestingPlaceEntityId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Locations_LocationEntityId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Trips_TripEntityId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "InterestingPlacesInfo");

            migrationBuilder.DropTable(
                name: "InterestingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Images_InterestingPlaceEntityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_LocationEntityId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_TripEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "InterestingPlaceEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LocationEntityId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "TripEntityId",
                table: "Images");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Images_LocationId",
                table: "Images",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_TripId",
                table: "Images",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Trips_TripId",
                table: "Images",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
