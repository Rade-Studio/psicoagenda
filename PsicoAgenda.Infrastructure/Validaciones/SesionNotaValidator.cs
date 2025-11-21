using FluentValidation;
using PsicoAgenda.Application.Dtos.SesionNota;

namespace PsicoAgenda.Infrastructure.Validaciones
{
    public class SesionNotaValidator : AbstractValidator<SesionNotaCreacion>
    {
        public SesionNotaValidator()
        {
            RuleFor(x => x.Contenido)
                .NotEmpty().WithMessage("El contenido de la nota es obligatorio")
                .MaximumLength(5000).WithMessage("El contenido es demasiado largo");
        }
    }
}
