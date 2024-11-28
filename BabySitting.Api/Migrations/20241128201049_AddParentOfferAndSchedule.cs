using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BabySitting.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddParentOfferAndSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Schedules",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MondayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    MondayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    MondayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    MondayNight = table.Column<bool>(type: "boolean", nullable: false),
                    TuesdayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    TuesdayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    TuesdayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    TuesdayNight = table.Column<bool>(type: "boolean", nullable: false),
                    WednesdayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    WednesdayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    WednesdayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    WednesdayNight = table.Column<bool>(type: "boolean", nullable: false),
                    ThursdayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    ThursdayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    ThursdayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    ThursdayNight = table.Column<bool>(type: "boolean", nullable: false),
                    FridayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    FridayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    FridayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    FridayNight = table.Column<bool>(type: "boolean", nullable: false),
                    SaturdayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    SaturdayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    SaturdayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    SaturdayNight = table.Column<bool>(type: "boolean", nullable: false),
                    SundayMorning = table.Column<bool>(type: "boolean", nullable: false),
                    SundayAfternoon = table.Column<bool>(type: "boolean", nullable: false),
                    SundayEvening = table.Column<bool>(type: "boolean", nullable: false),
                    SundayNight = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParentOffers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    AddressName = table.Column<string>(type: "text", nullable: false),
                    AddressLongitude = table.Column<double>(type: "double precision", nullable: false),
                    AddressLatitude = table.Column<double>(type: "double precision", nullable: false),
                    FamilySpeakingLanguages = table.Column<int[]>(type: "integer[]", nullable: false),
                    NumberOfChildren = table.Column<int>(type: "integer", nullable: false),
                    ChildrenAgeCategories = table.Column<int[]>(type: "integer[]", nullable: false),
                    ChildrenCharacteristics = table.Column<int[]>(type: "integer[]", nullable: false),
                    FamilyDescription = table.Column<string>(type: "text", nullable: false),
                    PreferebleSkills = table.Column<int[]>(type: "integer[]", nullable: false),
                    Currency = table.Column<int>(type: "integer", nullable: false),
                    Rate = table.Column<int>(type: "integer", nullable: false),
                    JobLocation = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentOffers_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "public",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParentOffers_ScheduleId",
                schema: "public",
                table: "ParentOffers",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParentOffers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "public");
        }
    }
}
