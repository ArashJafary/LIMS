using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LIMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCTORConflicts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playback_Record_RecordId",
                table: "Playback");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Record_MeetingId",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Playback",
                table: "Playback");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsRecord",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "MeetingId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "ParentMeetingId",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Meetings");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Playback");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Playback");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Playback");

            migrationBuilder.RenameTable(
                name: "Playback",
                newName: "Playbacks");

            migrationBuilder.RenameIndex(
                name: "IX_Playback_RecordId",
                table: "Playbacks",
                newName: "IX_Playbacks_RecordId");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playbacks",
                table: "Playbacks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Record_MeetingId",
                table: "Record",
                column: "MeetingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Playbacks_Record_RecordId",
                table: "Playbacks",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playbacks_Record_RecordId",
                table: "Playbacks");

            migrationBuilder.DropIndex(
                name: "IX_Record_MeetingId",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Playbacks",
                table: "Playbacks");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Playbacks",
                newName: "Playback");

            migrationBuilder.RenameIndex(
                name: "IX_Playbacks_RecordId",
                table: "Playback",
                newName: "IX_Playback_RecordId");

            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsRecord",
                table: "Meetings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MeetingId",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentMeetingId",
                table: "Meetings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDateTime",
                table: "Meetings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "Length",
                table: "Playback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Playback",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Playback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Playback",
                table: "Playback",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Record_MeetingId",
                table: "Record",
                column: "MeetingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playback_Record_RecordId",
                table: "Playback",
                column: "RecordId",
                principalTable: "Record",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRoles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
