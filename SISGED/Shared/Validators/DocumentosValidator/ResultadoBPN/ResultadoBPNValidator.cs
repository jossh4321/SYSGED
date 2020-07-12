using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ResultadoBPN
{
    public class ResultadoBPNValidator : AbstractValidator<ResultadoBPNDTO>
    {
        public ResultadoBPNValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoResultadoBPNValidator());
        }
    }
}
