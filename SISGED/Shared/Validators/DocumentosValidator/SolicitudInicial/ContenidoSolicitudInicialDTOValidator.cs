using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudInicial
{
    public class ContenidoSolicitudInicialDTOValidator : AbstractValidator<ContenidoSolicitudInicialDTO>
    {
        public ContenidoSolicitudInicialDTOValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar el titulo de la solicitud inicial");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar la descripción de la solicitud inicial obligatoriamente");
        }
    }
}
