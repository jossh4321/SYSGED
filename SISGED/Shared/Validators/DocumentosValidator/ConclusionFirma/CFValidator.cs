using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ConclusionFirma
{
    public class ADValidator : AbstractValidator<ConclusionFirmaDTO>
    {
        public ADValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoADValidator());
        }
    }
}
