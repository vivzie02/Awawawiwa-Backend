using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.awawawiwa.Migrations
{
    /// <inheritdoc />
    public partial class AddProfilePictureUrlToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "profilePictureUrl",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profilePictureUrl",
                table: "users");
        }
    }
}
