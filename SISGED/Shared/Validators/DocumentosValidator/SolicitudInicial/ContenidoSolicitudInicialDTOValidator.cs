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
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar el titulo de la soli");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar un Direccion de Oficio obligatoriamente");
        }
    }
}
