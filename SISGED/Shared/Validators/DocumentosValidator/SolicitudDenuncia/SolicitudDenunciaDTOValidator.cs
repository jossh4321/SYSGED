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
            RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
            RuleFor(x => x.tipodocumento).NotEmpty().WithMessage("Debe Ingresar un tipo de dcumento obligatoriamente");
            RuleFor(x => x.numerodocumento).NotEmpty().WithMessage("Debe Ingresar un numero de Documento obligatoriamente");
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudDenunciaDTOValidator());
        }
    }
}
