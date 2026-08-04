using CommunityReports.Application.DTOs.Instituciones.Requests;
using FluentValidation;

namespace CommunityReports.Application.Validators
{
    public sealed class InstitucionValidator : AbstractValidator<InstitucionRequestDto>
    {
        public InstitucionValidator()
        {
            RuleFor(x => x.Nombre).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Siglas).MaximumLength(30);
            RuleFor(x => x.Tipo).MaximumLength(60);
            RuleFor(x => x.Telefono).MaximumLength(20);
            RuleFor(x => x.Correo).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Correo));
            RuleFor(x => x.SitioWeb).MaximumLength(200);
        }
    }
}
