using Microsoft.EntityFrameworkCore.Migrations;

namespace Cryptocop.Software.API.Migrations
{
  public partial class FixedMisspelledVarInOrderItem : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "ProducrIdentifier",
          table: "OrderItems");

      migrationBuilder.AddColumn<string>(
          name: "ProductIdentifier",
          table: "OrderItems",
          nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "ProductIdentifier",
          table: "OrderItems");

      migrationBuilder.AddColumn<string>(
          name: "ProducrIdentifier",
          table: "OrderItems",
          type: "text",
          nullable: true);
    }
  }
}
