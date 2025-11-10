using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Uppbeat.Api.Data;
using Uppbeat.Api.Services;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Models;

namespace Uppbeat.Tests
{
    public class TrackServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public TrackServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task CreateAsync_AddsTrack()
        {
            using var context = new AppDbContext(_dbOptions);
            var service = new TrackService(context);

            var createDto = new TrackCreateDto
            {
                Name = "Test Track",
                FilePath = "file.mp3",
                DurationSeconds = 120,
                Genres = new() { "Pop", "Rock" }
            };

            var ownerId = Guid.NewGuid();
            var result = await service.CreateAsync(createDto, ownerId);

            Assert.NotNull(result);
            Assert.Equal("Test Track", result.Name);
            Assert.Equal(2, result.Genres.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTrack()
        {
            using var context = new AppDbContext(_dbOptions);
            var service = new TrackService(context);

            var artist = new User 
            { 
                Id = Guid.NewGuid(),
                Name = "Test Artist", 
                Email = "test@test.com", 
                PasswordHash = "hashedpassword"
            };
            context.Users.Add(artist);
            await context.SaveChangesAsync();

            var track = new Track
            {
                Name = "Existing Track",
                FilePath = "x.mp3",
                DurationSeconds = 60,
                Owner = artist
            };
            context.Tracks.Add(track);
            await context.SaveChangesAsync();

            var result = await service.GetByIdAsync(track.Id);

            Assert.NotNull(result);
            Assert.Equal(track.Name, result.Name);
            Assert.Equal(artist.Id, result.ArtistId);
        }
    }
}