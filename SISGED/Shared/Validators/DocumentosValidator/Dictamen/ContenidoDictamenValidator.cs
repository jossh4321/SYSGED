using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Dictamen
{

    public class ContenidoDictamenValidator : AbstractValidator<ContenidoDictamenDTO>
    {
        public ContenidoDictamenValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.nombredenunciante).NotEmpty().WithMessage("Debe Ingresar un nombre de denunciante obligatoriamente");
            RuleFor(x => x.conclusion).NotEmpty().WithMessage("Debe Ingresar una conclusión obligatoriamente");

            RuleForEach(x => x.observaciones).SetValidator(new ObservacionValidator());
            RuleFor(x => x.observaciones)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar una observación como minimo");

            RuleForEach(x => x.recomendaciones).SetValidator(new RecomendacionValidator());
            RuleFor(x => x.recomendaciones)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar una recomendación como minimo");
        }

    }
    public class ObservacionValidator : AbstractValidator<Observaciones>
    {
        public ObservacionValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar la observación obligatoriamente");
        }
    }

    public class RecomendacionValidator : AbstractValidator<Recomendaciones>
    {
        public RecomendacionValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar la recomendación obligatoriamente");
        }
    }
}
