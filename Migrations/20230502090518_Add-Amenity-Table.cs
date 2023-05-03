using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apartease_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenityTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resident_ApartmentId",
                table: "Resident");

            migrationBuilder.CreateTable(
                name: "Amenity",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmenityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmenityDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmenityContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmenityAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowWeekend = table.Column<bool>(type: "bit", nullable: false),
                    MininumBookingHour = table.Column<int>(type: "int", nullable: false),
                    ApartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenity", x => x.AmenityId);
                    table.ForeignKey(
                        name: "FK_Amenity_Apartment_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartment",
                        principalColumn: "ApartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resident_ApartmentId",
                table: "Resident",
                column: "ApartmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Amenity_ApartmentId",
                table: "Amenity",
                column: "ApartmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amenity");

            migrationBuilder.DropIndex(
                name: "IX_Resident_ApartmentId",
                table: "Resident");

            migrationBuilder.CreateIndex(
                name: "IX_Resident_ApartmentId",
                table: "Resident",
                column: "ApartmentId");
        }
    }
}
