using AutoFixture;
using Core.Common.Interfaces;
using Core.Domain.Entities;
using Core.UseCase.V1.UserOperation.Commands.Create;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Test.UseCase.V1.UserOperation.Commands.Create
{
    public class CreateUserCommandTest
    {
        private readonly Mock<IRepositoryEF> _mockRepository;

        private readonly Mock<ILogger<CreateUserCommandHandler>> _mockLogger;
        private CreateUserCommandHandler _handler;

        public CreateUserCommandTest()
        {
            _mockRepository = new Mock<IRepositoryEF>();
            _mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            _handler = new CreateUserCommandHandler(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesUserSuccessfully()
        {
            // Arrange

            var request = new CreateUserCommand
            {
                Name = "John Doe",
                Email = "john@example.com"
            };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockRepository.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal("User created successfully", response.Content.message);
        }

        [Fact]
        public async Task Handler_CreatePerson_UpdateDatabaseException()
        {
            // Arrange
            var request = new Fixture().Create<CreateUserCommand>();

            _mockRepository.Setup(_ => _.SaveChangesAsync()).ThrowsAsync(new DbUpdateException());

            // Act and Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(request, CancellationToken.None));

            _mockRepository.Verify(repo => repo.Insert(It.IsAny<User>()), Times.Once);
            _mockRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

    }
}
