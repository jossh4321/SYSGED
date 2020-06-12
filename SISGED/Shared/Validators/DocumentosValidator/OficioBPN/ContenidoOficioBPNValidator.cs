using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.OficioBPN
{
    public class ContenidoOficioBPNValidator : AbstractValidator<ContenidoOficioBPNDTO>
    {
        public ContenidoOficioBPNValidator(){
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.observacion).NotEmpty().WithMessage("Debe Ingresar una observacion obligatoriamente");
        }
    }
}
