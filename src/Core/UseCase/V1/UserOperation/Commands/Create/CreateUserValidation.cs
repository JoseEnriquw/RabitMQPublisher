using Core.Domain.Common;
using FluentValidation;

namespace Core.UseCase.V1.UserOperation.Commands.Create
{
    public class CreateUserValidation : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage(string.Format(ErrorMessage.NULL_VALUE, "{PropertyName}"))
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessage.EMPTY_VALUE, "{PropertyName}"))
                .MaximumLength(100)
                .WithMessage(string.Format(ErrorMessage.MAXIMUM_LENGTH, "{PropertyName}", "{MaxLength}"));

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage(string.Format(ErrorMessage.NULL_VALUE, "{PropertyName}"))
                .NotEmpty()
                .WithMessage(string.Format(ErrorMessage.EMPTY_VALUE, "{PropertyName}"))
                .MaximumLength(100)
                .WithMessage(string.Format(ErrorMessage.MAXIMUM_LENGTH, "{PropertyName}", "{MaxLength}"))
                .EmailAddress()
                .WithMessage(ErrorMessage.INVALID_MAIL);
        }
    }
}
