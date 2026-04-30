using EMS.API.Data;
using EMS.API.DTOs;
using EMS.API.Models;
using EMS.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static EMS.API.DTOs.EmployeeDtos;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private DbContextOptions<AppDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        // ✅ COMMON CONFIG (NO MOQ — BEST PRACTICE)
        private IConfiguration GetTestConfiguration()
        {
            var settings = new Dictionary<string, string>
            {
                { "Jwt:Key", "EmployeeManagementSysytemProjectByMahantheshGowdaHS17022003" },
                { "Jwt:Issuer", "EMS.API" },
                { "Jwt:Audience", "EMS.Client" },
                { "Jwt:ExpiryHours", "8" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings!)
                .Build();
        }

        // ✅ REGISTER - USER EXISTS
        [Test]
        public async Task RegisterAsync_WhenUsernameExists_ReturnsFailure()
        {
            var options = CreateNewContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Users.Add(new AppUser
                {
                    Username = "existing",
                    PasswordHash = "x",
                    Role = "Viewer",
                    CreatedAt = DateTime.UtcNow
                });
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var svc = new AuthService(context, GetTestConfiguration());

                var dto = new RegisterRequestDto
                {
                    Username = "existing",
                    Password = "p",
                    Role = "Viewer"
                };

                var res = await svc.RegisterAsync(dto);

                Assert.That(res.Success, Is.False);
                Assert.That(res.Message, Does.Contain("Username already exists"));
            }
        }

        // ✅ REGISTER - SUCCESS
        [Test]
        public async Task RegisterAsync_Success_ReturnsToken()
        {
            var options = CreateNewContextOptions();

            using (var context = new AppDbContext(options))
            {
                var svc = new AuthService(context, GetTestConfiguration());

                var dto = new RegisterRequestDto
                {
                    Username = "newuser",
                    Password = "Pass@123",
                    Role = "Viewer"
                };

                var res = await svc.RegisterAsync(dto);

                Assert.That(res.Success, Is.True);
                Assert.That(res.Username, Is.EqualTo("newuser"));
                Assert.That(res.Token, Is.Not.Null.And.Not.Empty);
            }
        }

        // ✅ LOGIN - INVALID PASSWORD
        [Test]
        public async Task LoginAsync_InvalidCredentials_ReturnsFailure()
        {
            var options = CreateNewContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Users.Add(new AppUser
                {
                    Username = "user1",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret"),
                    Role = "Viewer",
                    CreatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var svc = new AuthService(context, GetTestConfiguration());

                var dto = new LoginRequestDto
                {
                    Username = "user1",
                    Password = "wrong"
                };

                var res = await svc.LoginAsync(dto);

                Assert.That(res.Success, Is.False);
                Assert.That(res.Message, Does.Contain("Invalid username or password"));
            }
        }

        // ✅ LOGIN - SUCCESS
        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessAndToken()
        {
            var options = CreateNewContextOptions();

            using (var context = new AppDbContext(options))
            {
                context.Users.Add(new AppUser
                {
                    Username = "user2",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("secret123"),
                    Role = "Viewer",
                    CreatedAt = DateTime.UtcNow
                });

                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(options))
            {
                var svc = new AuthService(context, GetTestConfiguration());

                var dto = new LoginRequestDto
                {
                    Username = "user2",
                    Password = "secret123"
                };

                var res = await svc.LoginAsync(dto);

                Assert.That(res.Success, Is.True);
                Assert.That(res.Token, Is.Not.Null.And.Not.Empty);
            }
        }
    }
}