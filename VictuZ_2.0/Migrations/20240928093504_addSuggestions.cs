using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VictuZ_2._0.Migrations
{
    /// <inheritdoc />
    public partial class addSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_AspNetUsers_UserId",
                table: "Suggestions");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Suggestions",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestions_UserId",
                table: "Suggestions",
                newName: "IX_Suggestions_CreatedById");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_AspNetUsers_CreatedById",
                table: "Suggestions",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestions_AspNetUsers_CreatedById",
                table: "Suggestions");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Suggestions",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestions_CreatedById",
                table: "Suggestions",
                newName: "IX_Suggestions_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "News",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestions_AspNetUsers_UserId",
                table: "Suggestions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
