using CommunityReports.Application.DTOs.Ubicacion.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CreateMunicipioValidator : AbstractValidator<CreateMunicipioRequestDto>
    {
        public CreateMunicipioValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ProvinciaId).GreaterThan(0);
        }
    }
}
