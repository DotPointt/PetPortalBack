using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetPortalDAL.Migrations
{
    /// <inheritdoc />
    public partial class ProjectAddMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Budget",
                table: "Projects",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsBusinesProject",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StateOfProject",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsBusinesProject",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StateOfProject",
                table: "Projects");
        }
    }
}
