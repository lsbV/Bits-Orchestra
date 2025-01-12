using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Married = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "DateOfBirth", "Married", "Name", "Phone", "Salary" },
                values: new object[,]
                {
                    { 1, new DateOnly(1980, 1, 1), true, "John Doe", "+380563829166", 1000m },
                    { 2, new DateOnly(1985, 1, 1), false, "Jane Doe", "+380594629166", 2000m },
                    { 3, new DateOnly(1990, 1, 1), true, "Sammy Doe", "+380563829166", 3000m },
                    { 4, new DateOnly(1995, 1, 1), false, "Sally Doe", "+380888829166", 4000m },
                    { 5, new DateOnly(2000, 1, 1), true, "Sandy Doe", "+380563829166", 5000m },
                    { 6, new DateOnly(2005, 1, 1), false, "Victor Van", "+380563800566", 6000m },
                    { 7, new DateOnly(2010, 1, 1), true, "Maria Red", "+380568837666", 7000m },
                    { 8, new DateOnly(2015, 1, 1), false, "Pan Cat", "+380563826694", 8000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");
        }
    }
}
