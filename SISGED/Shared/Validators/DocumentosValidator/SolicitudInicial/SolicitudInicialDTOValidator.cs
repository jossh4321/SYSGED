using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudInicial
{
    public class SolicitudInicialDTOValidator : AbstractValidator<SolicitudInicialDTO>
    {
        public SolicitudInicialDTOValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudInicialDTOValidator());
        }
    }
}
