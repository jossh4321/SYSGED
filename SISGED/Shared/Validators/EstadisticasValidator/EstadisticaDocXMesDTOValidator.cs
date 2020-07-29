using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.EstadisticasValidator
{
    public class EstadisticaDocXMesDTOValidator : AbstractValidator<EstadisticaDocXMesDTO>
    {
        public EstadisticaDocXMesDTOValidator()
        {
            RuleFor(x => x.mes).NotEmpty().WithMessage("Debe seleccionar un mes obligatoriamente");
        }
    }
}
