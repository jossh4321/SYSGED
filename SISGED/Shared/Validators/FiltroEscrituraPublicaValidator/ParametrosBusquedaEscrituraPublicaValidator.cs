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
            RuleFor(x => x.direccionoficionotarial).Matches(@"^[A-aZ-z0-9áéíóú ]*[A-aZ-z0-9áéíóú]$").WithMessage("Debe ingresar una dirección válida").When(x => x.direccionoficionotarial != null && x.direccionoficionotarial !="");
            RuleFor(x => x.nombrenotario).Matches(@"^[A-aZ-z0-9áéíóú ]*[A-aZ-z0-9áéíóú]$").WithMessage("Debe ingresar un nombre válido").When(x => x.nombrenotario != null && x.nombrenotario != "");
            RuleFor(x => x.actojuridico).Matches(@"^[A-aZ-z0-9áéíóú ]*[A-aZ-z0-9áéíóú]$").WithMessage("Debe ingresar un acto jurídico válido").When(x => x.actojuridico !=null && x.actojuridico !="");
            RuleForEach(x => x.nombreotorgantes).SetValidator(new OtorgantesValidator());
        }
        
    }
    public class OtorgantesValidator : AbstractValidator<OtorganteDTO>
    {
        public OtorgantesValidator()
        {
            RuleFor(x => x.nombre).Matches(@"^[A-aZ-z0-9áéíóú ]*[A-aZ-z0-9áéíóú]$").WithMessage("Debe ingresar un nombre válido").When(x => x.nombre != null);
        }
    }
}
