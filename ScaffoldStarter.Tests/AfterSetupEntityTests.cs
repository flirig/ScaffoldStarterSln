using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ScaffoldStarter.Domain.Product;
using Xunit;
using Xunit.Abstractions;

namespace ScaffoldStarter.Tests
{
    public class AfterSetupEntityTests
    {
        private ProductContext _context;
        private ITestOutputHelper _output;

        public AfterSetupEntityTests(ITestOutputHelper output)
        {
            _context = new ProductContext();
            _output = output;
        }
        
        [Fact]
        public void Dependency_Not_Added()
        {
            // Arrange
            var testDeveloper = 
                _context.Developers.Add(new Developer
                {
                    FullName = "Tester Testov"
                });
            _context.SaveChanges();

            // Act
            _context.Bugs.AddRange( new []
            {
                new Bug
                {
                    Title = "Test bug",
                    DeveloperId = testDeveloper.Entity.Id,
                }
            });
            _context.SaveChanges();

            // Arrange
        }

        [Fact]
        public void Dependency_Added()
        {
            // Arrange
            var testDeveloper =
                _context.Developers.Add(new Developer
                {
                    FullName = "Tester Testov",
                    Bugs = new List<Bug>(new []
                    {
                        new Bug
                        {
                            Title = "Test bug",
                        },
                    })
                });

            // Act
            _context.SaveChanges();
            
            // Arrange
            var developer = _context.Developers
                .AsNoTracking()
                .Include(dev => dev.Bugs)
                .SingleOrDefault();
            developer.Bugs.Count
                .Should().Be(1);
            developer.Bugs.Single().Developer.Id
                .Should().Be(testDeveloper.Entity.Id);
        }
    }
}