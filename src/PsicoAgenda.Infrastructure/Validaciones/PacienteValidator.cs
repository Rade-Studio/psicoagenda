using FluentValidation;
using PsicoAgenda.Application.Dtos.Pacientes;

namespace PsicoAgenda.Infrastructure.Validaciones
{
    public class PacienteValidator : AbstractValidator<PacienteCreacion>
    {
        public PacienteValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("Los apellidos son obligatorios")
                .MaximumLength(150).WithMessage("Los apellidos no pueden superar 150 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El email es obligatorio")
                .EmailAddress().WithMessage("El email debe ser una dirección válida");

            RuleFor(x => x.FechaNacimiento)
                .LessThan(DateTime.UtcNow).WithMessage("La fecha de nacimiento debe ser en el pasado");

            RuleFor(x => x.Telefono)
                .MaximumLength(30).WithMessage("El teléfono no puede superar 30 caracteres");

            RuleFor(x => x.TagsJson)
                .MaximumLength(1000).WithMessage("Los tags son demasiado largos");
        }
    }
}
