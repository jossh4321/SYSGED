using FluentValidation;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class SolicitudExpedicionFirmaValidator : AbstractValidator<SolicitudExpedicionFirma>
    {
        public SolicitudExpedicionFirmaValidator()
        {
            RuleFor(x => x.contenido).SetValidator(new ContenidoSEFValidator());
        }
    }
}
