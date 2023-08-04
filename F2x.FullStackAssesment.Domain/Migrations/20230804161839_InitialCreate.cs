using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace F2x.FullStackAssesment.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AndresUrrego");

            migrationBuilder.CreateTable(
                name: "TblVehicleCount",
                schema: "AndresUrrego",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    strStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strDirection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    intQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblVehicleCount", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "TblVehicleCounterQueryHistory",
                schema: "AndresUrrego",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    intRegisters = table.Column<int>(type: "int", nullable: false),
                    dtDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblVehicleCounterQueryHistory", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "TblVehicleCounterWithAmount",
                schema: "AndresUrrego",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    strStation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strDirection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tHour = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    strCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dtDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dblAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TblVehicleCounterWithAmount", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TblVehicleCount",
                schema: "AndresUrrego");

            migrationBuilder.DropTable(
                name: "TblVehicleCounterQueryHistory",
                schema: "AndresUrrego");

            migrationBuilder.DropTable(
                name: "TblVehicleCounterWithAmount",
                schema: "AndresUrrego");
        }
    }
}
