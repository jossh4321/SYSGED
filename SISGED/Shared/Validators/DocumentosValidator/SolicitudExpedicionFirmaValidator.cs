using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class SolicitudExpedicionFirmaValidator : AbstractValidator<SolicitudExpedicionFirmaDTO>
    {
        public SolicitudExpedicionFirmaValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSEFValidator());
        }
    }
}
