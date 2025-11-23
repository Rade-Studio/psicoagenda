using FluentValidation;
using PsicoAgenda.Application.Dtos.Pacientes;

namespace PsicoAgenda.Infrastructure.Validaciones
{
    public class PacienteUpdateValidator : AbstractValidator<PacienteActualizacion>
    {
        public PacienteUpdateValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100);

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(150);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El email debe ser válido");

            RuleFor(x => x.Telefono)
                .MaximumLength(30);
        }
    }
}
