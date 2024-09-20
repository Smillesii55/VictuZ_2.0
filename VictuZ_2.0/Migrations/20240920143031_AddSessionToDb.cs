using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VictuZ_2._0.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "Id", "ActivityDate", "CreatedById", "Description", "EndDate", "LocationId", "Title" },
                values: new object[] { 1, new DateTime(2024, 9, 20, 16, 30, 31, 387, DateTimeKind.Local).AddTicks(9180), 1, "Dit is een voorbeeld", new DateTime(2024, 9, 20, 17, 30, 31, 387, DateTimeKind.Local).AddTicks(9231), 1, "Voorbeeld" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
