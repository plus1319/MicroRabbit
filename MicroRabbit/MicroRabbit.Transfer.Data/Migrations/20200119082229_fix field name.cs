using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroRabbit.Transfer.Data.Migrations
{
    public partial class fixfieldname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountBalance",
                table: "TransferLogs",
                newName: "TransferAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransferAmount",
                table: "TransferLogs",
                newName: "AccountBalance");
        }
    }
}
