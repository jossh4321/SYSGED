using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static SISGED.Shared.Entities.SolicitudExpedicionFirma;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class ContenidoSEFValidator : AbstractValidator<ContenidoSolicitudExpedicionFirmaDTO>
    {
        public ContenidoSEFValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar un título obligatoriamente");
            RuleFor(x => x.titulo).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "" && x.titulo.Trim().Length != 0);
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            //RuleFor(x => x.codigo).NotEmpty().WithMessage("Debe Ingresar un código obligatoriamente");
            //RuleFor(x => x.cliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
        }
    }
}
