using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Migrations
{
    public partial class UpdateVPSStates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RAM",
                table: "States");

            migrationBuilder.AlterColumn<double>(
                name: "CPU",
                table: "States",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<double>(
                name: "DiskRead",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DiskWrite",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetworkIn",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NetworkOut",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperationsRead",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OperationsWrite",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PacketsIn",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PacketsOut",
                table: "States",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiskRead",
                table: "States");

            migrationBuilder.DropColumn(
                name: "DiskWrite",
                table: "States");

            migrationBuilder.DropColumn(
                name: "NetworkIn",
                table: "States");

            migrationBuilder.DropColumn(
                name: "NetworkOut",
                table: "States");

            migrationBuilder.DropColumn(
                name: "OperationsRead",
                table: "States");

            migrationBuilder.DropColumn(
                name: "OperationsWrite",
                table: "States");

            migrationBuilder.DropColumn(
                name: "PacketsIn",
                table: "States");

            migrationBuilder.DropColumn(
                name: "PacketsOut",
                table: "States");

            migrationBuilder.AlterColumn<float>(
                name: "CPU",
                table: "States",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<float>(
                name: "RAM",
                table: "States",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
