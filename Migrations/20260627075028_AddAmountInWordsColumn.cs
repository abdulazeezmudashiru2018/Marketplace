using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPlace.Migrations
{
    /// <inheritdoc />
    public partial class AddAmountInWordsColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AmountInWords",
                table: "SalesHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInWords",
                table: "SalesHistories");
        }
    }
}
