using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Dictamen
{
    public class DictamenValidator : AbstractValidator<DictamenDTO>
    {
        public DictamenValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoDictamenValidator());
        }
    }
}
