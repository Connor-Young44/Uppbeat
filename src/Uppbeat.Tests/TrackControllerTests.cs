using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Uppbeat.Api.DTOs;
using Uppbeat.Api.Controllers;
using Uppbeat.Api.Interfaces;

namespace Uppbeat.Tests
{
    public class TrackControllerTests
    {
        private readonly Mock<ITrackService> _trackServiceMock = new();

        [Fact]
        public async Task Get_ReturnsOk_WithTrackDto()
        {
            var id = Guid.NewGuid();
            var dto = new TrackReadDto
            {
                Id = id,
                Name = "Test Track",
                FilePath = "file.mp3",
                DurationSeconds = 120
            };

            _trackServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(dto);
            var controller = new TracksController(_trackServiceMock.Object);

            var result = await controller.GetById(id);

            Assert.IsType<OkObjectResult>(result);
            var ok = (OkObjectResult)result;
            Assert.IsType<TrackReadDto>(ok.Value);
            var returned = (TrackReadDto)ok.Value;
            Assert.Equal(dto.Id, returned.Id);
            Assert.Equal(dto.Name, returned.Name);
            Assert.Equal(dto.FilePath, returned.FilePath);
            Assert.Equal(dto.DurationSeconds, returned.DurationSeconds);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithTrackDto()
        {
            var createDto = new TrackCreateDto
            {
                Name = "New",
                FilePath = "f.mp3",
                DurationSeconds = 90
            };

            var createdDto = new TrackReadDto
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                FilePath = createDto.FilePath,
                DurationSeconds = createDto.DurationSeconds
            };

            _trackServiceMock.Setup(s => s.CreateAsync(It.IsAny<TrackCreateDto>(), It.IsAny<Guid>()))
                             .ReturnsAsync(createdDto);

            var controller = new TracksController(_trackServiceMock.Object);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await controller.Create(createDto);

            Assert.IsType<CreatedAtActionResult>(result);
            var created = (CreatedAtActionResult)result;
            Assert.IsType<TrackReadDto>(created.Value);
            var returned = (TrackReadDto)created.Value;
            Assert.Equal(createdDto.Id, returned.Id);
            Assert.Equal(createdDto.Name, returned.Name);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            var id = Guid.NewGuid();

            _trackServiceMock
                .Setup(s => s.DeleteAsync(It.Is<Guid>(g => g == id), It.IsAny<Guid>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new TracksController(_trackServiceMock.Object);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var result = await controller.Delete(id);

            Assert.IsType<NoContentResult>(result);
        }
    }
}
