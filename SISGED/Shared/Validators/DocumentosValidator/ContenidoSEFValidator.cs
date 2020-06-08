using FluentValidation;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class ContenidoSEFValidator : AbstractValidator<ContenidoSolicitudExpedicionFirma>
    {
        public ContenidoSEFValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.codigo).NotEmpty().WithMessage("Debe Ingresar un código obligatoriamente");
            RuleFor(x => x.cliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
        }
    }
}
