using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Resolucion
{
    public class ResolucionValidator : AbstractValidator<ResolucionDTO>
    {
        public ResolucionValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoResolucionValidator());
        }
    }
}
