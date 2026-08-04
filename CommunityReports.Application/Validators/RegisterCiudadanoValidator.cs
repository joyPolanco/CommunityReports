using CommunityReports.Application.DTOs.Auth.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class RegisterCiudadanoValidator : AbstractValidator<RegisterCiudadanoRequestDto>
    {
        public RegisterCiudadanoValidator()
        {
            RuleFor(x => x.NombreUsuario).NotEmpty().Length(3, 50);
            RuleFor(x => x.Correo).NotEmpty().EmailAddress().MaximumLength(120);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres.");
            RuleFor(x => x.Cedula).NotEmpty().Length(9, 15);
            RuleFor(x => x.Nombres).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Apellidos).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Telefono).MaximumLength(20);
        }
    }
}
