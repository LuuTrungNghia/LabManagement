using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeviceBorrowingDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowedByUserId",
                table: "DeviceItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DeviceBorrowingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullNameId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceBorrowingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceBorrowingRequests_AspNetUsers_FullNameId",
                        column: x => x.FullNameId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceBorrowingRequests_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceBorrowingRequests_DeviceId",
                table: "DeviceBorrowingRequests",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceBorrowingRequests_FullNameId",
                table: "DeviceBorrowingRequests",
                column: "FullNameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceBorrowingRequests");

            migrationBuilder.DropColumn(
                name: "BorrowedByUserId",
                table: "DeviceItems");
        }
    }
}
