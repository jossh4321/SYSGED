using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudDenuncia
{
    public class ContenidoSolicitudDenunciaDTOValidator : AbstractValidator<ContenidoSolicitudDenunciaDTO>
    {
        public ContenidoSolicitudDenunciaDTOValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            //RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
        }
    }
}
