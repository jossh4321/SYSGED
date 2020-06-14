using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ConclusionFirma
{
    public class CFValidator : AbstractValidator<ConclusionFirmaDTO>
    {
        public CFValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoCFValidator());
        }
    }
}
