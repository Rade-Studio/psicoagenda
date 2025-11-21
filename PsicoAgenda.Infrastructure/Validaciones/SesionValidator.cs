using FluentValidation;
using PsicoAgenda.Application.Dtos.Sesiones;
using PsicoAgenda.Domain.Interfaces;

// Validate referenced GUIDs existence via unit of work

namespace PsicoAgenda.Infrastructure.Validaciones
{
    public class SesionValidator : AbstractValidator<SesionCreacion>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SesionValidator(IUnitOfWork unitOfWork)
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

            When(x => x.CitaId.HasValue, () =>
            {
                RuleFor(x => x.CitaId.Value)
                    .MustAsync(async (id, cancellationToken) =>
                    {
                        if (id == Guid.Empty) return false;
                        var cita = await _unitOfWork.Citas.SeleccionarPorId(id, cancellationToken);
                        return cita != null;
                    }).WithMessage("Cita no encontrada");
            });

            RuleFor(x => x.SoapSubj)
                .MaximumLength(2000).WithMessage("SoapSubj demasiado largo");

            RuleFor(x => x.Observaciones)
                .MaximumLength(4000).WithMessage("Observaciones demasiado largas");

            RuleFor(x => x.Analasis)
                .MaximumLength(4000).WithMessage("Análisis demasiado largo");

            RuleFor(x => x.PlanAccion)
                .MaximumLength(4000).WithMessage("Plan de acción demasiado largo");
        }
    }
}
