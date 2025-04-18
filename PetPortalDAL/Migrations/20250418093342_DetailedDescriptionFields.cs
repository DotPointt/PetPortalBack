using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPortalDAL.Migrations
{
    /// <inheritdoc />
    public partial class DetailedDescriptionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Plan",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requirements",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Result",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamDescription",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plan",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Requirements",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TeamDescription",
                table: "Projects");
        }
    }
}
