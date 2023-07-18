using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LIMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Update_Server_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingUser_Meetings_MeetingId",
                table: "MeetingUser");

            migrationBuilder.RenameColumn(
                name: "MeetingId",
                table: "MeetingUser",
                newName: "MeetingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingUser_Meetings_MeetingsId",
                table: "MeetingUser",
                column: "MeetingsId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeetingUser_Meetings_MeetingsId",
                table: "MeetingUser");

            migrationBuilder.RenameColumn(
                name: "MeetingsId",
                table: "MeetingUser",
                newName: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeetingUser_Meetings_MeetingId",
                table: "MeetingUser",
                column: "MeetingId",
                principalTable: "Meetings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
