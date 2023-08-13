using AutoFixture;
using Core.Common.Interfaces;
using Core.Domain.Common;
using Core.Domain.Entities;
using Core.UseCase.V1.UserOperation.Commands.Update;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Test.UseCase.V1.UserOperation.Commands.Update
{
    public class UpdateUserCommandTest
    {
        private readonly Mock<IRepositoryEF> _mockRepositoryEF;
        private readonly Mock<ILogger<UpdateUserCommandHandler>> _mockLogger;
        private readonly UpdateUserCommandHandler _handler;

        public UpdateUserCommandTest()
        {
            _mockRepositoryEF = new Mock<IRepositoryEF>();
            _mockLogger = new Mock<ILogger<UpdateUserCommandHandler>>();
            _handler = new UpdateUserCommandHandler(_mockRepositoryEF.Object, _mockLogger.Object);
        }

        // Assuming you have already set up the necessary dependencies and imports for xUnit and Moq.

        [Fact]
        public async Task Handle_ValidCommand_UserUpdatedSuccessfully()
        {
            // Arrange
            var user = new Fixture().Create<User>();
            user.Id = 1;

            _mockRepositoryEF.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                                .ReturnsAsync(user);

            var request= new UpdateUserCommand { Id = 1, Name = "Updated Name", Email = "updated@example.com" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockRepositoryEF.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mockRepositoryEF.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(request.Name, response.Content.Name);
            Assert.Equal(request.Email, response.Content.Email);
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateUserCommand { Id = 1, Name = "Updated Name", Email = "updated@example.com" };

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _mockRepositoryEF.Verify(repo => repo.Update(It.IsAny<User>()), Times.Never);
            _mockRepositoryEF.Verify(repo => repo.SaveChangesAsync(), Times.Never);

            Assert.NotNull(response);
            Assert.Null(response.Content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(response.Notifications[0].Message, string.Format(ErrorMessage.NOT_FOUND_GET_BY_ID, request.Id, nameof(User)));
        }

        [Fact]
        public async Task Handle_RepositoryError_ReturnsException()
        {
            // Arrange
            var existingUser = new User { Id = 1, Name = "John Doe", Email = "john@example.com" };

            _mockRepositoryEF.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<User, bool>>>()))
                                .ReturnsAsync(existingUser);
            _mockRepositoryEF.SetupSequence(_ => _.SaveChangesAsync()).ThrowsAsync(new DbUpdateException());

            var request = new UpdateUserCommand { Id = 1, Name = "Updated Name", Email = "updated@example.com" };

            // Act and  Assert
            await Assert.ThrowsAsync<DbUpdateException>(() => _handler.Handle(request, CancellationToken.None));

            _mockRepositoryEF.Verify(repo => repo.Update(It.IsAny<User>()), Times.Once);
            _mockRepositoryEF.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
     

    }
}
