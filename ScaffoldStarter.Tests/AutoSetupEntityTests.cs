using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScaffoldStarter.Domain.Product;
using Xunit;
using Xunit.Abstractions;

namespace ScaffoldStarter.Tests
{
    public class AutoSetupEntityTests
    {
        private ProductContext _context;
        private ITestOutputHelper _output;

        public AutoSetupEntityTests(ITestOutputHelper output)
        {
            _context = new ProductContext();
            _output = output;
        }
        [Fact]
        public void DeveloperName_Should_BeUpdated()
        {
            // Arrange
            _context.Developers.Add(
                new Developer
                {
                    FullName = "Tester Testov",
                    Tasks = new List<Task>(new []
                    {
                        new Task
                        {
                            Title = "Task Title",
                            Status = 0,
                        }
                    })
                    
                });
            _context.SaveChanges();

            // Act
            _context.Database.ExecuteSqlRaw("UPDATE Developers SET FullName = 'Tester Testerov' WHERE Id=1");
            _context.SaveChanges();
            var developer = _context.Developers
                    .AsNoTracking()
                    .Include( developer => developer.Tasks)
                    .SingleOrDefault();

            // Assert
            developer.FullName.Should().Be("Tester Testerov");
            developer.Tasks.Count().Should().Be(1);
        }
    }
}