using Core.Domain.Common;
using FluentValidation;

namespace Core.UseCase.V1.UserOperation.Commands.Update
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserCommand>
    {

        public UpdateUserValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(string.Format(ErrorMessage.MUST_BE_A_POSITIVE_NUMBER, "{PropertyName}"));

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
