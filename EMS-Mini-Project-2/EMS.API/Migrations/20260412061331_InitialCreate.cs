using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "Designation", "Email", "FirstName", "JoinDate", "LastName", "Phone", "Salary", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Software Engineer", "amit.sharma@example.com", "Amit", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sharma", "9876543210", 60000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "HR Manager", "priya.verma@example.com", "Priya", new DateTime(2021, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verma", "9876543211", 50000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "Accountant", "rahul.mehta@example.com", "Rahul", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mehta", "9876543212", 45000m, "Inactive", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "Marketing Executive", "sneha.reddy@example.com", "Sneha", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Reddy", "9876543213", 40000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "Operations Manager", "karan.patel@example.com", "Karan", new DateTime(2020, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patel", "9876543214", 55000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Backend Developer", "anjali.gupta@example.com", "Anjali", new DateTime(2022, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gupta", "9876543215", 65000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "Financial Analyst", "vikram.singh@example.com", "Vikram", new DateTime(2023, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Singh", "9876543216", 48000m, "Inactive", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "Recruiter", "neha.kapoor@example.com", "Neha", new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kapoor", "9876543217", 42000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Frontend Developer", "arjun.das@example.com", "Arjun", new DateTime(2022, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Das", "9876543218", 62000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "Content Strategist", "pooja.nair@example.com", "Pooja", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nair", "9876543219", 43000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "Supervisor", "ravi.kumar@example.com", "Ravi", new DateTime(2021, 4, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kumar", "9876543220", 47000m, "Inactive", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "HR Executive", "meera.iyer@example.com", "Meera", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Iyer", "9876543221", 41000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "Auditor", "suresh.yadav@example.com", "Suresh", new DateTime(2022, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yadav", "9876543222", 52000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "QA Engineer", "kavya.shetty@example.com", "Kavya", new DateTime(2023, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Shetty", "9876543223", 58000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "Logistics Manager", "manoj.joshi@example.com", "Manoj", new DateTime(2021, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Joshi", "9876543224", 53000m, "Active", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$G.Mee/dzyiO0zpNSgqTI6uSW4ciR5aus3XAp7c6rrIruUC7uB/OL2", "Admin", "admin" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "$2a$11$5.P8cMgYOBvhK9rp0RJOZOBmHiDOoV/Q8LgipBT0LjOtGwX8qhay6", "Viewer", "viewer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
