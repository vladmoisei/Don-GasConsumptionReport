using Microsoft.EntityFrameworkCore.Migrations;

namespace Don_GasConsumtionReport.Migrations
{
    public partial class IndexModelWithValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GazValue",
                table: "IndexModels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GazValue",
                table: "IndexModels");
        }
    }
}
