using Bogus;
using Moq;
using Xunit;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Dev.Challenge.Application.Features.Courier.Commands.UpdateDriverLicenseImage;
using Dev.Challenge.Application.Service;
using MediatR;
using Microsoft.AspNetCore.Http.Internal;

namespace Dev.Challenge.Test.Application.Handlers.Courier.Commands
{
    public class UpdateDriverLicenseImageCommandHandlerTests
    {
        private readonly Mock<ICourierService> _courierServiceMock;
        private readonly UpdateDriverLicenseImageCommandHandler _handler;
        private readonly Faker _faker;

        public UpdateDriverLicenseImageCommandHandlerTests()
        {
            _courierServiceMock = new Mock<ICourierService>();
            _handler = new UpdateDriverLicenseImageCommandHandler(_courierServiceMock.Object);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_SuccessfullyUpdatesDriverLicenseImage()
        {
            // Arrange
            var command = new UpdateDriverLicenseImageCommand
            {
                Id = Guid.NewGuid(),
                File = CreateFakeFormFile("image.png", "image/png")
            };

            _courierServiceMock.Setup(c => c.UpdateDriverLicenseImageAsync(It.IsAny<Guid>(), It.IsAny<Stream>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _courierServiceMock.Verify(c => c.UpdateDriverLicenseImageAsync(command.Id, It.IsAny<Stream>(), command.File.FileName), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenCourierServiceFails()
        {
            // Arrange
            var command = new UpdateDriverLicenseImageCommand
            {
                Id = Guid.NewGuid(),
                File = CreateFakeFormFile("image.png", "image/png")
            };

            _courierServiceMock.Setup(c => c.UpdateDriverLicenseImageAsync(It.IsAny<Guid>(), It.IsAny<Stream>(), It.IsAny<string>())).ThrowsAsync(new Exception("Falha no serviço"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Falha no serviço", exception.Message);
        }

        private IFormFile CreateFakeFormFile(string fileName, string contentType)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("Fake file content");
            writer.Flush();
            stream.Position = 0;

            return new FormFile(stream, 0, stream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }

}
