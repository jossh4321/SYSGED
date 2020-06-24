using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.AperturamientoDisciplinario
{
    public class ADValidator : AbstractValidator<AperturamientoDisciplinarioDTO>
    {
        public ADValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoADValidator());
        }
    }
}
