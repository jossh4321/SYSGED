using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudPBN
{
    public class ContenidoSolicitudBPNDTOValidator: AbstractValidator<ContenidoSolicitudBPNDTO>
    {
        public ContenidoSolicitudBPNDTOValidator()
        {
            RuleFor(x => x.actojuridico).NotEmpty().WithMessage("Debe ingresar un acto jurídico obligatoriamente");
            RuleFor(x => x.direccionoficio).NotEmpty().WithMessage("Debe ingresar un dirección de oficio obligatoriamente");
            RuleFor(x => x.tipoprotocolo).NotEmpty().WithMessage("Debe ingresar un tipo de protocolo obligatoriamente");
            RuleFor(x => x.fecharealizacion).NotEmpty().WithMessage("Debe ingresar una fecha obligatoriamente");
            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario()).WithMessage("Debe seleccionar un notario obligatoriamente");
        }
    }
}
