using FluentValidation;
using SISGED.Shared.Entities;
using SISGED.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.FiltroEscrituraPublicaValidator
{
    public class ParametrosBusquedaEscrituraPublicaValidator: AbstractValidator<ParametrosBusquedaEscrituraPublicaDTO>
    {
        public ParametrosBusquedaEscrituraPublicaValidator()
        {
            RuleFor(x => x.direccionoficionotarial).NotEmpty().WithMessage("Debe ingresar una dirección");
            RuleFor(x => x.nombrenotario).NotEmpty().WithMessage("Debe ingresar un nombre");
            RuleFor(x => x.actojuridico).NotEmpty().WithMessage("Debe ingresar un acto jurídico");
            RuleForEach(x => x.nombreotorgantes).SetValidator(new OtorgantesValidator());
        }
        
    }
    public class OtorgantesValidator : AbstractValidator<OtorganteDTO>
    {
        public OtorgantesValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe ingresar un nombre");
        }
    }
}
