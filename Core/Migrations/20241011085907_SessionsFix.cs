using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class SessionsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAttended",
                table: "SessionRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasCancelled",
                table: "SessionRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPresent",
                table: "SessionRegistrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasAttended",
                table: "SessionRegistrations");

            migrationBuilder.DropColumn(
                name: "HasCancelled",
                table: "SessionRegistrations");

            migrationBuilder.DropColumn(
                name: "IsPresent",
                table: "SessionRegistrations");
        }
    }
}
