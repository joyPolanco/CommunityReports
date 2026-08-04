using CommunityReports.Application.DTOs.Users.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class ChangePasswordValidator : AbstractValidator<ChangePasswordRequestDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.PasswordActual).NotEmpty();
            RuleFor(x => x.PasswordNueva).NotEmpty().MinimumLength(8)
                .WithMessage("La nueva contraseña debe tener al menos 8 caracteres.");
        }
    }
}
