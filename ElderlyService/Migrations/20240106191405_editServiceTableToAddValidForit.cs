using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElderlyService.Migrations
{
    /// <inheritdoc />
    public partial class editServiceTableToAddValidForit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "valid",
                table: "services",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valid",
                table: "services");
        }
    }
}
