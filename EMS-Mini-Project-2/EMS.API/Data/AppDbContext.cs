using EMS.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMS.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            // Suppress the PendingModelChangesWarning as seed data is deterministic
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Constraints for employee email 
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            // Unique Constraints for AppUser Username
            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Static seed data for users
            var createdAtDate = new DateTime(2024, 1, 1);

            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$11$J5wRkkMG7pcL3c8vY3E56OPqcRqNxkzR.LdIl7Pt7oNvgJnV3xNEK",
                    Role = "Admin",
                    CreatedAt = createdAtDate
                },
                new AppUser
                {
                    Id = 2,
                    Username = "viewer",
                    PasswordHash = "$2a$11$pF0V5lJQr7YdvJ1M0Lg5zOzK0vxP1Y5Z8xQ3B6C8D9E0F1G2H3I4J",
                    Role = "Viewer",
                    CreatedAt = createdAtDate
                }
            );

            // Employee seed data
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, FirstName = "Amit", LastName = "Sharma", Email = "amit.sharma@example.com", Phone = "9876543210", Department = "Engineering", Designation = "Software Engineer", Salary = 60000m, JoinDate = new DateTime(2022, 1, 1), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 2, FirstName = "Priya", LastName = "Verma", Email = "priya.verma@example.com", Phone = "9876543211", Department = "HR", Designation = "HR Manager", Salary = 50000m, JoinDate = new DateTime(2021, 5, 10), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 3, FirstName = "Rahul", LastName = "Mehta", Email = "rahul.mehta@example.com", Phone = "9876543212", Department = "Finance", Designation = "Accountant", Salary = 45000m, JoinDate = new DateTime(2023, 3, 15), Status = "Inactive", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 4, FirstName = "Sneha", LastName = "Reddy", Email = "sneha.reddy@example.com", Phone = "9876543213", Department = "Marketing", Designation = "Marketing Executive", Salary = 40000m, JoinDate = new DateTime(2023, 6, 1), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 5, FirstName = "Karan", LastName = "Patel", Email = "karan.patel@example.com", Phone = "9876543214", Department = "Operations", Designation = "Operations Manager", Salary = 55000m, JoinDate = new DateTime(2020, 11, 20), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 6, FirstName = "Anjali", LastName = "Gupta", Email = "anjali.gupta@example.com", Phone = "9876543215", Department = "Engineering", Designation = "Backend Developer", Salary = 65000m, JoinDate = new DateTime(2022, 7, 12), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 7, FirstName = "Vikram", LastName = "Singh", Email = "vikram.singh@example.com", Phone = "9876543216", Department = "Finance", Designation = "Financial Analyst", Salary = 48000m, JoinDate = new DateTime(2023, 1, 25), Status = "Inactive", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 8, FirstName = "Neha", LastName = "Kapoor", Email = "neha.kapoor@example.com", Phone = "9876543217", Department = "HR", Designation = "Recruiter", Salary = 42000m, JoinDate = new DateTime(2023, 8, 5), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 9, FirstName = "Arjun", LastName = "Das", Email = "arjun.das@example.com", Phone = "9876543218", Department = "Engineering", Designation = "Frontend Developer", Salary = 62000m, JoinDate = new DateTime(2022, 9, 18), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 10, FirstName = "Pooja", LastName = "Nair", Email = "pooja.nair@example.com", Phone = "9876543219", Department = "Marketing", Designation = "Content Strategist", Salary = 43000m, JoinDate = new DateTime(2023, 10, 10), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 11, FirstName = "Ravi", LastName = "Kumar", Email = "ravi.kumar@example.com", Phone = "9876543220", Department = "Operations", Designation = "Supervisor", Salary = 47000m, JoinDate = new DateTime(2021, 4, 14), Status = "Inactive", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 12, FirstName = "Meera", LastName = "Iyer", Email = "meera.iyer@example.com", Phone = "9876543221", Department = "HR", Designation = "HR Executive", Salary = 41000m, JoinDate = new DateTime(2023, 2, 2), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 13, FirstName = "Suresh", LastName = "Yadav", Email = "suresh.yadav@example.com", Phone = "9876543222", Department = "Finance", Designation = "Auditor", Salary = 52000m, JoinDate = new DateTime(2022, 5, 30), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 14, FirstName = "Kavya", LastName = "Shetty", Email = "kavya.shetty@example.com", Phone = "9876543223", Department = "Engineering", Designation = "QA Engineer", Salary = 58000m, JoinDate = new DateTime(2023, 7, 7), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate },
                new Employee { Id = 15, FirstName = "Manoj", LastName = "Joshi", Email = "manoj.joshi@example.com", Phone = "9876543224", Department = "Operations", Designation = "Logistics Manager", Salary = 53000m, JoinDate = new DateTime(2021, 12, 12), Status = "Active", CreatedAt = createdAtDate, UpdatedAt = createdAtDate }
            );
        }
    }
}