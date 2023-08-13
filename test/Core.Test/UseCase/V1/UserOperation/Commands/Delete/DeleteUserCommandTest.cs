using AutoFixture;
using Core.Common.Interfaces;
using Core.Domain.Common;
using Core.Domain.Entities;
using Core.UseCase.V1.UserOperation.Commands.Delete;
using Core.UseCase.V1.UserOperation.Commands.Update;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using System.Net;

namespace Core.Test.UseCase.V1.UserOperation.Commands.Delete
{
    public class DeleteUserCommandTest
    {
        private readonly Mock<IRepositoryEF> _mockIRepository;
        private readonly Mock<ILogger<DeleteUserCommandHandler>> _mockLogger;
        private readonly DeleteUserCommandHandler _handler;

        public DeleteUserCommandTest()
        {
            _mockIRepository = new Mock<IRepositoryEF>();
            _mockLogger = new Mock<ILogger<DeleteUserCommandHandler>>();
            _handler = new DeleteUserCommandHandler(_mockIRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_UserFound_DeletesUser()
        {
            // Arrange
            var userToDelete = new Fixture().Create<User>();
            userToDelete.Id = 1;
            var request = new DeleteUserCommand { Id = 1 };

            _mockIRepository.SetupSequence(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                          .ReturnsAsync(userToDelete);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockIRepository.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Once);
            _mockIRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(string.Format(ErrorMessage.USER_DELETED, userToDelete.Name), response.Content.message);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsNotFoundResponse()
        {
            // Arrange
            var request = new DeleteUserCommand { Id = 1 };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockIRepository.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Never);
            _mockIRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(response.Notifications[0].Message, string.Format(ErrorMessage.NOT_FOUND_GET_BY_ID, request.Id, nameof(User)));
        }

        [Fact]
        public async Task Handle_RepositoryError_ReturnsException()
        {
            // Arrange
            var existingUser = new Fixture().Create<User>();

            _mockIRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                                .ReturnsAsync(existingUser);

            _mockIRepository.SetupSequence(_ => _.SaveChangesAsync()).ThrowsAsync(new DbUpdateException());

            var request = new DeleteUserCommand { Id = 1 };

            // Act and  Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(request, CancellationToken.None));

            _mockIRepository.Verify(repo => repo.Delete(It.IsAny<User>()), Times.Once);
            _mockIRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

    }
}
