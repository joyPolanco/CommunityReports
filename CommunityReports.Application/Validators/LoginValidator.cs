using CommunityReports.Application.DTOs.Auth.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Correo).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
