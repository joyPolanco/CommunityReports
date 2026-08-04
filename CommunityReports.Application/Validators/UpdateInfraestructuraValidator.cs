using CommunityReports.Application.DTOs.Infraestructura.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class UpdateInfraestructuraValidator : AbstractValidator<UpdateInfraestructuraRequestDto>
    {
        public UpdateInfraestructuraValidator() => RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
    }
}
