using CommunityReports.Application.DTOs.Ubicacion.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CreateSectorValidator : AbstractValidator<CreateSectorRequestDto>
    {
        public CreateSectorValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(120);
            RuleFor(x => x.MunicipioId).GreaterThan(0);
        }
    }
}
