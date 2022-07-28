using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VirtoCommerce.AuditLog.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogRecord",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OperationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ObjectType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ObjectId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldObject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogRecordField",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuditLogRecordId = table.Column<string>(type: "nvarchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogRecordField", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogRecordField_AuditLogRecord_AuditLogRecordId",
                        column: x => x.AuditLogRecordId,
                        principalTable: "AuditLogRecord",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogRecordField_AuditLogRecordId",
                table: "AuditLogRecordField",
                column: "AuditLogRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogRecordField");

            migrationBuilder.DropTable(
                name: "AuditLogRecord");
        }
    }
}
