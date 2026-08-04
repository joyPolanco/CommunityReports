using CommunityReports.Application.DTOs.Categorias.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class CategoriaValidator : AbstractValidator<CategoriaRequestDto>
    {
        public CategoriaValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(80);
            RuleFor(x => x.Color).NotEmpty().MaximumLength(20);
            RuleFor(x => x.TiempoRespuesta).GreaterThan(0);
        }
    }
}
