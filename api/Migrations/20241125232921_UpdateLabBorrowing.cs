using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLabBorrowing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupStudents_LabBorrowingRequests_LabBorrowingRequestId",
                table: "GroupStudents");

            migrationBuilder.DropIndex(
                name: "IX_GroupStudents_LabBorrowingRequestId",
                table: "GroupStudents");

            migrationBuilder.DropColumn(
                name: "LabBorrowingRequestId",
                table: "GroupStudents");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "LabBorrowingRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "LabBorrowingRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "LabBorrowingRequests");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "LabBorrowingRequests");

            migrationBuilder.AddColumn<int>(
                name: "LabBorrowingRequestId",
                table: "GroupStudents",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudents_LabBorrowingRequestId",
                table: "GroupStudents",
                column: "LabBorrowingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupStudents_LabBorrowingRequests_LabBorrowingRequestId",
                table: "GroupStudents",
                column: "LabBorrowingRequestId",
                principalTable: "LabBorrowingRequests",
                principalColumn: "Id");
        }
    }
}
