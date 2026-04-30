using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using EMS.API.Services;
using EMS.API.Repositories;
using EMS.API.Models;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<IEmployeeRepository> _repoMock = null!;
        private EmployeeService _service = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
        }



        [Test]
        public void AddAsync_EmailExists_ThrowsException()
        {
            var emp = new Employee { Email = "exists@test.com" };

            _repoMock.Setup(r => r.EmailExistsAsync(emp.Email, null)).ReturnsAsync(true);

            Assert.ThrowsAsync<Exception>(() => _service.AddAsync(emp));
        }

        [Test]
        public async Task AddAsync_ValidEmployee_SetsTimestampsAndCallsRepo()
        {
            var emp = new Employee { Email = "new@test.com" };

            _repoMock.Setup(r => r.EmailExistsAsync(emp.Email, null)).ReturnsAsync(false);

            await _service.AddAsync(emp);

            _repoMock.Verify(r => r.AddAsync(emp), Times.Once);
            Assert.That(emp.CreatedAt, Is.Not.EqualTo(default(DateTime)));
            Assert.That(emp.UpdatedAt, Is.Not.EqualTo(default(DateTime)));
        }

        [Test]
        public void UpdateAsync_NotFound_ThrowsException()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

            var dto = new Employee { FirstName = "X" };

            Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
        }

        [Test]
        public void UpdateAsync_EmailExists_ThrowsException()
        {
            var existing = new Employee { Id = 1, Email = "old@test.com", FirstName = "Old" };
            var dto = new Employee { Email = "taken@test.com", FirstName = "New" };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.EmailExistsAsync(dto.Email, 1)).ReturnsAsync(true);

            Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, dto));
        }

        [Test]
        public async Task UpdateAsync_Valid_UpdatesFieldsAndCallsRepo()
        {
            var existing = new Employee
            {
                Id = 1,
                FirstName = "Old",
                LastName = "Name",
                Email = "old@test.com",
                Phone = "123",
                Department = "HR",
                Designation = "Dev",
                Salary = 100,
                JoinDate = DateTime.UtcNow.AddYears(-1),
                Status = "Active"
            };

            var dto = new Employee
            {
                FirstName = "New",
                LastName = "Name2",
                Email = "new@test.com",
                Phone = "999",
                Department = "IT",
                Designation = "Senior",
                Salary = 200,
                JoinDate = DateTime.UtcNow,
                Status = "Inactive"
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.EmailExistsAsync(dto.Email, 1)).ReturnsAsync(false);

            await _service.UpdateAsync(1, dto);

            _repoMock.Verify(r => r.UpdateAsync(It.Is<Employee>(e =>
                e.Id == 1 &&
                e.FirstName == "New" &&
                e.LastName == "Name2" &&
                e.Email == "new@test.com" &&
                e.Phone == "999" &&
                e.Department == "IT" &&
                e.Designation == "Senior" &&
                e.Salary == 200 &&
                e.Status == "Inactive" &&
                e.UpdatedAt != default(DateTime)
            )), Times.Once);
        }
    }
}
