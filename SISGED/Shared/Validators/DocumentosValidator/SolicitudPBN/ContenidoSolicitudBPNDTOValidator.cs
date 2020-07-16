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
            RuleFor(x => x.actojuridico).NotEmpty().WithMessage("Debe Ingresar un Acto Jurídico obligatoriamente");
            RuleFor(x => x.direccionoficio).NotEmpty().WithMessage("Debe Ingresar un Direccion de Oficio obligatoriamente");
            RuleFor(x => x.tipoprotocolo).NotEmpty().WithMessage("Debe Ingresar un Tipo de Protocolo obligatoriamente");
            RuleFor(x => x.fecharealizacion).NotEmpty().WithMessage("Debe Ingresar una fecha obligatoriamente");
            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario()).WithMessage("Debe seleccionar un Notario Obligatoriamente");
        }
    }
}
