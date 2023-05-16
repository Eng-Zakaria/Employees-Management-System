using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class IdentityRecieved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DBCC CHECKIDENT('Departments',RESEED,0)");
            migrationBuilder.Sql(@"DBCC CHECKIDENT('Employees',RESEED,0)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
