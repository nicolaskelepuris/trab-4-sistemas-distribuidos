using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    public partial class AddRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AppUser_FromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AppUser_ToId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser");

            migrationBuilder.RenameTable(
                name: "AppUser",
                newName: "AppUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "AppUsers",
                type: "nchar(11)",
                fixedLength: true,
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_AppUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AppUserId",
                table: "Transactions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Cpf",
                table: "AppUsers",
                column: "Cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AppUsers_AppUserId",
                table: "Transactions",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AppUsers_FromId",
                table: "Transactions",
                column: "FromId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AppUsers_ToId",
                table: "Transactions",
                column: "ToId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AppUsers_AppUserId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AppUsers_FromId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AppUsers_ToId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AppUserId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_Cpf",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AppUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUser",
                table: "AppUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AppUser_FromId",
                table: "Transactions",
                column: "FromId",
                principalTable: "AppUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AppUser_ToId",
                table: "Transactions",
                column: "ToId",
                principalTable: "AppUser",
                principalColumn: "Id");
        }
    }
}
