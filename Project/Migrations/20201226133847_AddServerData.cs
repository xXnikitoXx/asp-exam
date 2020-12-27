using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class AddServerData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "VPSs");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "VPSs");

            migrationBuilder.AddColumn<string>(
                name: "ServerDataId",
                table: "VPSs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ServersData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPv4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPv4DNSPointer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPv4Blocked = table.Column<bool>(type: "bit", nullable: false),
                    IPv6 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPv6DNSPointer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPv6Blocked = table.Column<bool>(type: "bit", nullable: false),
                    Distro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistroVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VPSId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServersData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VPSs_ServerDataId",
                table: "VPSs",
                column: "ServerDataId",
                unique: true,
                filter: "[ServerDataId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VPSs_ServersData_ServerDataId",
                table: "VPSs",
                column: "ServerDataId",
                principalTable: "ServersData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VPSs_ServersData_ServerDataId",
                table: "VPSs");

            migrationBuilder.DropTable(
                name: "ServersData");

            migrationBuilder.DropIndex(
                name: "IX_VPSs_ServerDataId",
                table: "VPSs");

            migrationBuilder.DropColumn(
                name: "ServerDataId",
                table: "VPSs");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "VPSs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "VPSs",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }
    }
}
