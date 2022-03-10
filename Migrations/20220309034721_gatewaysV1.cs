using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable
/// <summary>
/// Migrations provide us with a way to control schema changes in the database and 
/// Entity Framework Core Migrations updates the database schema instead of creating 
/// a new database
/// </summary>
namespace MusalaGatewaysSysAdmin.Migrations
{
    public partial class gatewaysV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gateway",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxClientNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateway", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PeripheralDevice",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceVendor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DtDeviceCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Online = table.Column<bool>(type: "bit", nullable: false),
                    GatewayID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeripheralDevice", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PeripheralDevice_Gateway_GatewayID",
                        column: x => x.GatewayID,
                        principalTable: "Gateway",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeripheralDevice_GatewayID",
                table: "PeripheralDevice",
                column: "GatewayID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeripheralDevice");

            migrationBuilder.DropTable(
                name: "Gateway");
        }
    }
}
