using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators
{
    public class OficioDesignacionNotarioValidator : AbstractValidator<OficioDesignacionNotarioDTO>
    {
        public OficioDesignacionNotarioValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoODNValidator());
        }
    }
}
