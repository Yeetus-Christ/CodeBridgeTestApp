using CodeBridgeTestApp.Data;
using CodeBridgeTestApp.Dtos;
using CodeBridgeTestApp.Models;
using CodeBridgeTestApp.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace UnitTests
{
    public class DogServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DogService _dogService;

        public DogServiceTests()
        {
            _context = CreateDbContext();
            _dogService = new DogService(_context);
        }

        [Fact]
        public async Task GetDogsAsync_ReturnsAllDogsWithDefaultSorting()
        {
            // Arrange
            await SeedDogsAsync();

            // Act
            var result = await _dogService.GetDogsAsync("name", "asc", 1, 10);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Jessy", result.First().Name);
        }

        [Fact]
        public async Task GetDogsAsync_ReturnsDogsSortedByWeightDescending()
        {
            // Arrange
            await SeedDogsAsync();

            // Act
            var result = await _dogService.GetDogsAsync("weight", "desc", 1, 10);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Neo", result.First().Name);
        }

        [Fact]
        public async Task GetDogsAsync_ReturnsPagedResults()
        {
            // Arrange
            await SeedMultipleDogsAsync(15);

            // Act
            var result = await _dogService.GetDogsAsync("name", "asc", 2, 10);

            // Assert
            Assert.Equal(5, result.Count());
        }

        [Fact]
        public async Task CreateDogAsync_ReturnsError_WhenNameIsEmpty()
        {
            // Arrange
            await SeedDogsAsync();
            var newDog = new DogCreateDto { Name = string.Empty, Color = "white", TailLength = 20, Weight = 30 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);

            // Assert
            Assert.Equal("Name or color cannot be empty.", result);
        }

        [Fact]
        public async Task CreateDogAsync_ReturnsError_WhenColorIsEmpty()
        {
            // Arrange
            await SeedDogsAsync();
            var newDog = new DogCreateDto { Name = "Buddy", Color = string.Empty, TailLength = 20, Weight = 30 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);

            // Assert
            Assert.Equal("Name or color cannot be empty.", result);
        }

        [Fact]
        public async Task CreateDogAsync_ReturnsError_WhenDogWithSameNameExists()
        {
            // Arrange
            await SeedDogsAsync();
            var newDog = new DogCreateDto { Name = "Neo", Color = "white", TailLength = 20, Weight = 30 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);

            // Assert
            Assert.Equal("A dog with the same name already exists.", result);
        }

        [Fact]
        public async Task CreateDogAsync_ReturnsError_WhenTailLengthIsNegative()
        {
            // Arrange
            var newDog = new DogCreateDto { Name = "Buddy", Color = "brown", TailLength = -5, Weight = 15 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);

            // Assert
            Assert.Equal("Tail length must be a positive number.", result);
        }

        [Fact]
        public async Task CreateDogAsync_ReturnsError_WhenWeightIsNegative()
        {
            // Arrange
            var newDog = new DogCreateDto { Name = "Buddy", Color = "brown", TailLength = 10, Weight = -3 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);

            // Assert
            Assert.Equal("Weight must be a positive number.", result);
        }

        [Fact]
        public async Task CreateDogAsync_SuccessfullyCreatesDog()
        {
            // Arrange
            var newDog = new DogCreateDto { Name = "Buddy", Color = "brown", TailLength = 10, Weight = 15 };

            // Act
            var result = await _dogService.CreateDogAsync(newDog);
            var createdDog = await _context.Dogs.FirstOrDefaultAsync(d => d.Name == "Buddy");

            // Assert
            Assert.Null(result);
            Assert.NotNull(createdDog);
            Assert.Equal("Buddy", createdDog.Name);
        }

        private AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

            return new AppDbContext(options);
        }

        private async Task SeedDogsAsync()
        {
            _context.Dogs.AddRange(
                new Dog { Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
                new Dog { Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
            );
            await _context.SaveChangesAsync();
        }

        private async Task SeedMultipleDogsAsync(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                _context.Dogs.Add(new Dog
                {
                    Name = $"Dog{i}",
                    Color = "brown",
                    TailLength = 10 + i,
                    Weight = 15 + i
                });
            }
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}