using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Apelacion
{
    public class ApelacionValidator : AbstractValidator<ApelacionDTO>
    {
        public ApelacionValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoApelacionValidator());
        }
    }
}
