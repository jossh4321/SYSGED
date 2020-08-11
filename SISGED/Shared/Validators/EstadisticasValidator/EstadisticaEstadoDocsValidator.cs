using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.EstadisticasValidator
{
    class EstadisticaEstadoDocsValidator : AbstractValidator<EstadisticaEstDocsUsuario>
    {
        public EstadisticaEstadoDocsValidator()
        {
            RuleFor(x => x.mes).NotEmpty().WithMessage("Debe seleccionar un mes obligatoriamente");
            RuleFor(x => x.usuario).Must(usuario => usuario != null && usuario != new usuario_unwind())
                .WithMessage("Debe seleccionar un usuario obligatoriamente");
        }
    }
}
