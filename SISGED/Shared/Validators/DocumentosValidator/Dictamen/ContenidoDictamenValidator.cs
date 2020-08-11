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
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar un título obligatoriamente");
            RuleFor(x => x.titulo).Matches("@^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            RuleFor(x => x.nombredenunciante).NotEmpty().WithMessage("Debe ingresar un nombre de denunciante obligatoriamente");
            RuleFor(x => x.conclusion).NotEmpty().WithMessage("Debe ingresar una conclusión obligatoriamente");

            RuleForEach(x => x.observaciones).SetValidator(new ObservacionValidator());
            RuleFor(x => x.observaciones)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar una observación como mínimo");

            RuleForEach(x => x.recomendaciones).SetValidator(new RecomendacionValidator());
            RuleFor(x => x.recomendaciones)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar una recomendación como mínimo");

            //RuleFor(x => x.fechaemision).Must(BeAValidDate).WithMessage("Fecha de Emision Invalida");
        }
        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
    public class ObservacionValidator : AbstractValidator<Observaciones>
    {
        public ObservacionValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar la observación obligatoriamente");
        }
    }

    public class RecomendacionValidator : AbstractValidator<Recomendaciones>
    {
        public RecomendacionValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar la recomendación obligatoriamente");
        }
    }
}
