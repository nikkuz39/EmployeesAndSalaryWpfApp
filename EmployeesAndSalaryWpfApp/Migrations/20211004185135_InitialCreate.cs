using Microsoft.EntityFrameworkCore.Migrations;

namespace EmployeesAndSalaryWpfApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NamePost = table.Column<string>(type: "TEXT", nullable: true),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    RatePlus = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Staffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NameStaff = table.Column<string>(type: "TEXT", nullable: true),
                    Dateofemployment = table.Column<string>(type: "TEXT", nullable: true),
                    Headofdepartment = table.Column<string>(type: "TEXT", nullable: true),
                    Paycheck = table.Column<decimal>(type: "TEXT", nullable: false),
                    PostId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staffers_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Staffers_PostId",
                table: "Staffers",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staffers");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
