using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ConclusionFirma
{
    public class ContenidoCFValidator : AbstractValidator<ContenidoConclusionFirmaDTO>
    {
        public ContenidoCFValidator()
        {
            RuleFor(x => x.idescriturapublica).Must(escritura => escritura != null && escritura != new EscrituraPublica())
                .WithMessage("Debe seleccionar una escritura pública obligatoriamente");
        }
    }
}
