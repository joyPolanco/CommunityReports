using CommunityReports.Application.DTOs.Infraestructura.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CreateInfraestructuraValidator : AbstractValidator<CreateInfraestructuraRequestDto>
    {
        public CreateInfraestructuraValidator()
        {
            RuleFor(x => x.Tipo).IsInEnum();
            RuleFor(x => x.DireccionId).GreaterThan(0);
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Codigo).NotEmpty().MaximumLength(40);
        }
    }
}
