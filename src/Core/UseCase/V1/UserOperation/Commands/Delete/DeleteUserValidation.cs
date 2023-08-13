using Core.Domain.Common;
using FluentValidation;

namespace Core.UseCase.V1.UserOperation.Commands.Delete
{
    public class DeleteUserValidation : AbstractValidator<DeleteUserCommand>
    {

        public DeleteUserValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(string.Format(ErrorMessage.MUST_BE_A_POSITIVE_NUMBER, "{PropertyName}"));
        }
    }
}
