using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BabySitting.Api.Migrations
{
    /// <inheritdoc />
    public partial class added_postalCode_in_parentOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                schema: "public",
                table: "ParentOffers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                schema: "public",
                table: "ParentOffers");
        }
    }
}
