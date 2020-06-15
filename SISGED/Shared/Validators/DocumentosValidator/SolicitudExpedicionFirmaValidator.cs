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
            RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
            RuleFor(x => x.tipodocumento).NotEmpty().WithMessage("Debe Ingresar un tipo de dcumento obligatoriamente");
            RuleFor(x => x.numerodocumento).NotEmpty().WithMessage("Debe Ingresar un numero de Documento obligatoriamente");
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSEFValidator());
        }
    }
}
