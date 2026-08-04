using CommunityReports.Application.DTOs.Auth.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class RegisterEmpleadoValidator : AbstractValidator<RegisterEmpleadoRequestDto>
    {
        public RegisterEmpleadoValidator()
        {
            RuleFor(x => x.NombreUsuario).NotEmpty().Length(3, 50);
            RuleFor(x => x.Correo).NotEmpty().EmailAddress().MaximumLength(120);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
                .WithMessage("La contraseña debe tener al menos 8 caracteres.");
            RuleFor(x => x.InstitucionId).GreaterThan(0);
            RuleFor(x => x.Cargo).NotEmpty().MaximumLength(80);
            RuleFor(x => x.CodigoEmpleado).NotEmpty().MaximumLength(40);
            RuleFor(x => x.Telefono).MaximumLength(20);
        }
    }
}
