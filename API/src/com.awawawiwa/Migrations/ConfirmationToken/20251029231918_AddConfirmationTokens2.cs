using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace com.awawawiwa.Migrations.ConfirmationToken
{
    /// <inheritdoc />
    public partial class AddConfirmationTokens2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_confirmationTokens",
                table: "confirmationTokens");

            migrationBuilder.RenameTable(
                name: "confirmationTokens",
                newName: "confirmation_tokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_confirmation_tokens",
                table: "confirmation_tokens",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_confirmation_tokens",
                table: "confirmation_tokens");

            migrationBuilder.RenameTable(
                name: "confirmation_tokens",
                newName: "confirmationTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_confirmationTokens",
                table: "confirmationTokens",
                column: "id");
        }
    }
}
