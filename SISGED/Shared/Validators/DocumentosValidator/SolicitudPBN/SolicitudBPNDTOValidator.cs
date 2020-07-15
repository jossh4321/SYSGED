using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using SISGED.Shared.DTOs;
using System.Text.RegularExpressions;
using SISGED.Shared.Entities;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudPBN
{
    public class SolicitudBPNDTOValidator: AbstractValidator<SolicitudBPNDTO>
    {
        public SolicitudBPNDTOValidator()
        {
            
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudBPNDTOValidator());
        }
    }
}
