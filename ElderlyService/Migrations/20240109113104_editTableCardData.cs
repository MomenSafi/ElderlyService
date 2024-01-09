using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElderlyService.Migrations
{
    /// <inheritdoc />
    public partial class editTableCardData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Caregivers_CaregiverId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CaregiverId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CaregiverId",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaregiverId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CaregiverId",
                table: "Cards",
                column: "CaregiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Caregivers_CaregiverId",
                table: "Cards",
                column: "CaregiverId",
                principalTable: "Caregivers",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
