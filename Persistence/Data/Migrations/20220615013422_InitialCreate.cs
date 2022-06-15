using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AppUser_FromId",
                        column: x => x.FromId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_AppUser_ToId",
                        column: x => x.ToId,
                        principalTable: "AppUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromId",
                table: "Transactions",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToId",
                table: "Transactions",
                column: "ToId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
