using FluentValidation;
using PsicoAgenda.Application.Dtos.Citas;
using PsicoAgenda.Domain.Interfaces;

// Use unit of work to validate existence of related entities

namespace PsicoAgenda.Infrastructure.Validaciones
{
    public class CitaValidator : AbstractValidator<CitaCreacion>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CitaValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.PacienteId)
                .NotEmpty().WithMessage("El paciente es obligatorio")
                .MustAsync(async (id, cancellationToken) =>
                {
                    if (id == Guid.Empty) return false;

                    var paciente = await _unitOfWork.Pacientes.SeleccionarPorId(id, cancellationToken);
                    return paciente != null;
                }).WithMessage("Paciente no encontrado");

            RuleFor(x => x.FechaInicio)
                .NotEmpty().WithMessage("Fecha de inicio obligatoria");

            RuleFor(x => x.FechaFin)
                .NotEmpty().WithMessage("Fecha de fin obligatoria")
                .GreaterThan(x => x.FechaInicio).WithMessage("La fecha de fin debe ser posterior a la fecha de inicio");

            RuleFor(x => x.Modo)
                .IsInEnum().WithMessage("Modo de cita inválido");

            RuleFor(x => x.Estado)
                .IsInEnum().WithMessage("Estado de cita inválido");

            When(x => x.Modo.ToString().Equals("Presencial", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.UbicacionLink)
                    .NotEmpty().WithMessage("La ubicación es obligatoria para citas presenciales");
            });
        }
    }
}
