using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.awawawiwa.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "confirmed",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmed",
                table: "users");
        }
    }
}
