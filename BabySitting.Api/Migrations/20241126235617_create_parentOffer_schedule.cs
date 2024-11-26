using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabySitting.Api.Migrations
{
    /// <inheritdoc />
    public partial class create_parentOffer_schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_Name",
                schema: "identity",
                table: "AspNetRoles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_Name",
                schema: "identity",
                table: "AspNetRoles");
        }
    }
}
