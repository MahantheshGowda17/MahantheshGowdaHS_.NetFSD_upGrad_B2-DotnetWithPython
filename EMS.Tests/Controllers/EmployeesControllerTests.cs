using NUnit.Framework;
using Moq;
using EMS.API.Services;
using EMS.API.Repositories;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.Tests.Services
{
    [TestFixture]
    public class EmployeeControllersTests
    {
        private Mock<IEmployeeRepository> _repoMock;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IEmployeeRepository>();
            _service = new EmployeeService(_repoMock.Object);
        }

        // ✅ GetById - Success
        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsEmployee()
        {
            var emp = new Employee { Id = 1, FirstName = "John" };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(emp);

            var result = await _service.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.FirstName, Is.EqualTo("John"));
        }

        // ✅ GetById - Not Found
        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetByIdAsync(99))
                     .ReturnsAsync((Employee?)null);

            var result = await _service.GetByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        // ✅ Add - Email Exists
        [Test]
        public void AddAsync_EmailExists_ThrowsException()
        {
            var emp = new Employee { Email = "test@test.com" };

            _repoMock.Setup(r => r.EmailExistsAsync(emp.Email, null))
                     .ReturnsAsync(true);

            Assert.ThrowsAsync<Exception>(() => _service.AddAsync(emp));
        }

        // ✅ Add - Success
        [Test]
        public async Task AddAsync_ValidEmployee_CallsRepo()
        {
            var emp = new Employee { Email = "new@test.com" };

            _repoMock.Setup(r => r.EmailExistsAsync(emp.Email, null))
                     .ReturnsAsync(false);

            await _service.AddAsync(emp);

            _repoMock.Verify(r => r.AddAsync(emp), Times.Once);
        }

        // ✅ Delete - Not Found
        [Test]
        public void DeleteAsync_InvalidId_ThrowsException()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync((Employee?)null);

            Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
        }

        // ✅ Delete - Success
        [Test]
        public async Task DeleteAsync_ValidId_CallsRepo()
        {
            var emp = new Employee { Id = 1 };

            _repoMock.Setup(r => r.GetByIdAsync(1))
                     .ReturnsAsync(emp);

            await _service.DeleteAsync(1);

            _repoMock.Verify(r => r.DeleteAsync(emp), Times.Once);
        }
    }
}