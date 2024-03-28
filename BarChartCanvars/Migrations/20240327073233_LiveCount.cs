using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarChartCanvars.Migrations
{
    public partial class LiveCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentInteractions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referrer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentInteractions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Browser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BrowserCanJavaScript = table.Column<bool>(type: "bit", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrowserHtml5VideoCanVideo = table.Column<bool>(type: "bit", nullable: false),
                    BrowserHtml5AudioCanAudio = table.Column<bool>(type: "bit", nullable: false),
                    CanTouchScreen = table.Column<bool>(type: "bit", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: true),
                    DeviceModelName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceOperatingSystemModel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceOperatingSystemVendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceVendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HardwareDisplayWidth = table.Column<int>(type: "int", nullable: false),
                    HardwareDisplayHeight = table.Column<int>(type: "int", nullable: false),
                    DeviceIsSmartphone = table.Column<bool>(type: "bit", nullable: false),
                    IsBot = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageEventData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageEventData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Logout = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WhoIsInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SaveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    MetroCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BusinessName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DNS = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhoIsInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterationData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceInfoId = table.Column<int>(type: "int", nullable: true),
                    GeoIPId = table.Column<int>(type: "int", nullable: true),
                    PageEventId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterationData_DeviceDetails_DeviceInfoId",
                        column: x => x.DeviceInfoId,
                        principalTable: "DeviceDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterationData_PageEventData_PageEventId",
                        column: x => x.PageEventId,
                        principalTable: "PageEventData",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InterationData_WhoIsInformation_GeoIPId",
                        column: x => x.GeoIPId,
                        principalTable: "WhoIsInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterationData_DeviceInfoId",
                table: "InterationData",
                column: "DeviceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_InterationData_GeoIPId",
                table: "InterationData",
                column: "GeoIPId");

            migrationBuilder.CreateIndex(
                name: "IX_InterationData_PageEventId",
                table: "InterationData",
                column: "PageEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentInteractions");

            migrationBuilder.DropTable(
                name: "InterationData");

            migrationBuilder.DropTable(
                name: "UserLoginDetails");

            migrationBuilder.DropTable(
                name: "DeviceDetails");

            migrationBuilder.DropTable(
                name: "PageEventData");

            migrationBuilder.DropTable(
                name: "WhoIsInformation");
        }
    }
}
