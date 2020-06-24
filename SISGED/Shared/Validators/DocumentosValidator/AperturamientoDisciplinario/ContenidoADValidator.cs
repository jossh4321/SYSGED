using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.AperturamientoDisciplinario
{
    public class ContenidoADValidator : AbstractValidator<ContenidoAperturamientoDisciplinarioDTO>
    {
        public ContenidoADValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.nombredenunciante).NotEmpty().WithMessage("Debe Ingresar un nombre de denunciante obligatoriamente");
            RuleFor(x => x.lugaraudiencia).NotEmpty().WithMessage("Debe Ingresar el lugar de la audiencia obligatoriamente");  
            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario())
                .WithMessage("Debe seleccionar un Notario Obligatoriamente");

            RuleForEach(x => x.participantes).SetValidator(new ParticipanteValidator());
            RuleFor(x => x.participantes)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un participante como minimo");

            RuleForEach(x => x.hechosimputados).SetValidator(new HechoValidator());
            RuleFor(x => x.hechosimputados)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un Hecho inputado como minimo");



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

    public class HechoValidator : AbstractValidator<Hecho>
    {
        public HechoValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar el hecho obligatoriamente");
        }
    }
}
