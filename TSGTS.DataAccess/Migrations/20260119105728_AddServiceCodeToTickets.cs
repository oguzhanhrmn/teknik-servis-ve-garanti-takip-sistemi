using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TSGTS.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceCodeToTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServisKodu",
                table: "ServisKayitlari",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServisKodu",
                table: "ServisKayitlari");
        }
    }
}
