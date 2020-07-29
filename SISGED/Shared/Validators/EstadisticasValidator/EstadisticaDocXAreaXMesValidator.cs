using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.EstadisticasValidator
{
    public class EstadisticaDocXAreaXMesValidator: AbstractValidator<EstadisticaDocXMesXAreaDTO>
    {
        public EstadisticaDocXAreaXMesValidator()
        {
            RuleFor(x => x.mes).NotEmpty().WithMessage("Debe seleccionar un mes obligatoriamente");
            RuleFor(x => x.area).NotEmpty().WithMessage("Debe seleccionar un area obligatoriamente");
        }
    }
}
