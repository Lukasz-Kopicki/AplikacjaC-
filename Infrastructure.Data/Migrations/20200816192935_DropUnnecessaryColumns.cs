using Microsoft.EntityFrameworkCore.Migrations;


namespace Infrastructure.Data.Migrations
{
    public partial class DropUnnecessaryColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_ID",
                table: "Workplaces");

            migrationBuilder.DropColumn(
                name: "Company_ID",
                table: "Workplaces");

            migrationBuilder.DropColumn(
                name: "Workplace_ID",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "Worker_ID",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Address_ID",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "City_ID",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "City_ID",
                table: "Addresses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Address_ID",
                table: "Workplaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Company_ID",
                table: "Workplaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Workplace_ID",
                table: "Workers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Worker_ID",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Address_ID",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "City_ID",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "City_ID",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
