using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projektarbete_Bokningssystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Jupiter");

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Telescope");

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Science");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Studierum 1");

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Studierum 2");

            migrationBuilder.UpdateData(
                table: "StudyRooms",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Studierum 3");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
