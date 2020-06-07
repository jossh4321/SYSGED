using FluentValidation;
using SISGED.Shared.Entities;
using SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators
{
    public class OficioDesignacionNotarioValidator : AbstractValidator<OficioDesignacionNotario>
    {
        public OficioDesignacionNotarioValidator()
        {
            RuleFor(x => x.contenido).SetValidator(new ContenidoODNValidator());
        }
    }
}
