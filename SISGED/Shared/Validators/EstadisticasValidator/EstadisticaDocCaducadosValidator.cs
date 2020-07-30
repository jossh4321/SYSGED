using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.EstadisticasValidator
{
    public class EstadisticaDocCaducadosValidator : AbstractValidator<EstadisticaDocCaducados>
    {
        public EstadisticaDocCaducadosValidator()
        {
            RuleFor(x => x.mes).NotEmpty().WithMessage("Debe seleccionar un mes obligatoriamente");
            RuleFor(x => x.dni).NotEmpty().WithMessage("Debe ingresar un dni obligatoriamente");
        }
    }
}
