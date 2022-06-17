using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    public partial class UpdateOrganRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "Organ",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OrganRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Organ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganRequests_AppUsers_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrganRequests_RequesterId",
                table: "OrganRequests",
                column: "RequesterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganRequests");

            migrationBuilder.DropColumn(
                name: "Organ",
                table: "Transactions");

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
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
                name: "IX_Requests_RequesterId",
                table: "Requests",
                column: "RequesterId");
        }
    }
}
