using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.OficioBPN
{
    public class OficioBPNValidator: AbstractValidator<OficioBPNDTO>
    {
        public OficioBPNValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoOficioBPNValidator());
        }
    }
}
