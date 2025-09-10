using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lekping.server.Migrations
{
    /// <inheritdoc />
    public partial class MedsInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    BrandName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    GenericName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    StrengthValue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 6, nullable: false),
                    StrengthUnit = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Form = table.Column<int>(type: "INTEGER", nullable: false),
                    PackageSize = table.Column<int>(type: "INTEGER", nullable: false),
                    Ean = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meds_Ean",
                table: "Meds",
                column: "Ean");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meds");
        }
    }
}
