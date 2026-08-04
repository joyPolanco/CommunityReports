using CommunityReports.Application.DTOs.Ubicacion.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CreateProvinciaValidator : AbstractValidator<CreateProvinciaRequestDto>
    {
        public CreateProvinciaValidator() => RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
    }
}
