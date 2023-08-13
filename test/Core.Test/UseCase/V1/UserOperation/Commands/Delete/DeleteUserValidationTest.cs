using Core.UseCase.V1.UserOperation.Commands.Delete;
using FluentValidation.TestHelper;

namespace Core.Test.UseCase.V1.UserOperation.Commands.Delete
{
    public class DeleteUserValidationTest
    {
        private readonly DeleteUserValidation _validator;
        public DeleteUserValidationTest()=> _validator = new DeleteUserValidation();

        [Fact]
        public void Request_NotNull()
        {
            // Arrange
            var request = new DeleteUserCommand()
            {
                Id = 0
            };
            // Act
            var response = _validator.TestValidate(request);
            //Assert
            response.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorCode("GreaterThanValidator");
            response.ShouldHaveAnyValidationError();
        }
    }
}
