using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudDenuncia
{
    public class SolicitudDenunciaDTOValidator:AbstractValidator<SolicitudDenunciaDTO>
    {
        public SolicitudDenunciaDTOValidator()
        {
            RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
            RuleFor(x => x.tipodocumento).NotEmpty().WithMessage("Debe Ingresar un tipo de documento obligatoriamente");
            RuleFor(x => x.numerodocumento).NotEmpty().WithMessage("Debe Ingresar un numero de Documento obligatoriamente");
            RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{8}$").WithMessage("El número de documento esta mal escrito").When(x => x.tipodocumento == "DNI");
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudDenunciaDTOValidator());
        }
    }
}
