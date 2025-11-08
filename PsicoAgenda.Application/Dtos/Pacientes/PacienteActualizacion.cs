using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsicoAgenda.Application.Dtos.Pacientes;

public record PacienteActualizacion(
string Nombre,
string Apellidos,
string Email,
string Telefono,
string ContactoEmergencia,
string TagsJson,
DateTime FechaNacimiento
);



