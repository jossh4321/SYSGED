using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ConclusionFirma
{
    public class ContenidoADValidator : AbstractValidator<ContenidoConclusionFirmaDTO>
    {
        public ContenidoADValidator()
        {
            RuleFor(x => x.idescriturapublica).Must(escritura => escritura != null && escritura != new EscrituraPublica())
                .WithMessage("Debe seleccionar una escritura pública obligatoriamente");
        }
    }
}
