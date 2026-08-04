using CommunityReports.Application.DTOs.Ubicacion.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CreateDireccionValidator : AbstractValidator<CreateDireccionRequestDto>
    {
        public CreateDireccionValidator()
        {
            RuleFor(x => x.SectorId).GreaterThan(0);
            RuleFor(x => x.Calle).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CodigoPostal).MaximumLength(20);
            RuleFor(x => x.Latitud).InclusiveBetween(-90, 90).When(x => x.Latitud.HasValue);
            RuleFor(x => x.Longitud).InclusiveBetween(-180, 180).When(x => x.Longitud.HasValue);
        }
    }
}
