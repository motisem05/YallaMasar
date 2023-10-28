using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaMasar.Migrations
{
    public partial class LinkLocationsWithImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                table: "InterestingPlaces",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "InterestingPlaceId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_InterestingPlaces_LocationId",
                table: "InterestingPlaces",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_InterestingPlaceId",
                table: "Images",
                column: "InterestingPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_LocationId",
                table: "Images",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_InterestingPlaces_InterestingPlaceId",
                table: "Images",
                column: "InterestingPlaceId",
                principalTable: "InterestingPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterestingPlaces_Locations_LocationId",
                table: "InterestingPlaces",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_InterestingPlaces_InterestingPlaceId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_Locations_LocationId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_InterestingPlaces_Locations_LocationId",
                table: "InterestingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_InterestingPlaces_LocationId",
                table: "InterestingPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Images_InterestingPlaceId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_LocationId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "InterestingPlaces");

            migrationBuilder.DropColumn(
                name: "InterestingPlaceId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "LocationId",
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
    }
}
