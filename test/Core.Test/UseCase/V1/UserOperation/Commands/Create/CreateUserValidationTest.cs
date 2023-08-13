using Core.UseCase.V1.UserOperation.Commands.Create;
using FluentValidation.TestHelper;

namespace Core.Test.UseCase.V1.UserOperation.Commands.Create
{
    public class CreateUserValidationTest
    {
        private readonly CreateUserValidation _validator;

        public CreateUserValidationTest()=> _validator = new CreateUserValidation();

        [Fact]
        public void Request_NotNull()
        {
            // Arrange
            var request = new CreateUserCommand()
            {
                Name = null,
                Email = null
            };

            // Act
            var response = _validator.TestValidate(request);

            //Assert
            response.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("NotNullValidator");

            response.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode("NotNullValidator");

            response.ShouldHaveAnyValidationError();
        }


        [Fact]
        public void Request_NotEmpty()
        {
            // Arrange
            var request = new CreateUserCommand()
            {
                Name = "",
                Email = ""
            };

            // Act
            var response = _validator.TestValidate(request);

            //Assert
            response.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("NotEmptyValidator");

            response.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorCode("NotEmptyValidator");


            response.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Request_Email_Invalid()
        {
            // Arrange
            var request = new CreateUserCommand()
            {
                Name = "Test",
                Email = "test"
            };
            
            // Act
            var response = _validator.TestValidate(request);
            
            //Assert
            response.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorCode("EmailValidator");
            
            response.ShouldHaveAnyValidationError();
        }

        [Fact]
        public void Request_MaximumLength()
        {
            // Arrange
            var request = new CreateUserCommand()
            {
                Name = "Testaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                Email = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
            };

            // Act
            var response = _validator.TestValidate(request);

            //Assert
            response.ShouldHaveValidationErrorFor(x => x.Email)
                .WithErrorCode("MaximumLengthValidator");

            response.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorCode("MaximumLengthValidator");
        
            response.ShouldHaveAnyValidationError();
        }
    }
}
