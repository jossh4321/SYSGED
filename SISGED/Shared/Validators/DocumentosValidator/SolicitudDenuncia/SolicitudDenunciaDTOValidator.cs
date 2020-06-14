using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudDenuncia
{
    public class SolicitudDenunciaDTOValidator:AbstractValidator<SolicitudDenunciaDTO>
    {
        public SolicitudDenunciaDTOValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudDenunciaDTOValidator());
        }
    }
}
