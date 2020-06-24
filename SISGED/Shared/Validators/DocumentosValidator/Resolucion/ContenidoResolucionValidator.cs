using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Resolucion
{
    public class ContenidoResolucionValidator : AbstractValidator<ContenidoResolucionDTO>
    {
        public ContenidoResolucionValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.sancion).NotEmpty().WithMessage("Debe Ingresar una sanción obligatoriamente");
            //DOCUMENTO OBLIGATORIO
            RuleForEach(x => x.participantes).SetValidator(new ParticipanteValidator());
            RuleFor(x => x.participantes)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un participante como minimo");

            RuleFor(x => x.fechainicioaudiencia).Must(BeAValidDate).WithMessage("Fecha de Inicio Invalida");
            RuleFor(x => x.fechafinaudiencia).Must(BeAValidDate).WithMessage("Fecha de finalizacion Invalida");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
    public class ParticipanteValidator : AbstractValidator<Participante>
    {
        public ParticipanteValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe Ingresar el nombre obligatoriamente");
        }
    }
}
