using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.AperturamientoDisciplinario
{
    public class ContenidoADValidator : AbstractValidator<ContenidoAperturamientoDisciplinarioDTO>
    {
        public ContenidoADValidator()
        {
            RuleFor(x => x.titulo).NotEmpty()
                .WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleForEach(x => x.participantes).SetValidator(new ParticipanteValidator());
        }
    }
    public class ParticipanteValidator : AbstractValidator<Participante>
    {
        public ParticipanteValidator()
        {
            // All your other validation rules for Guitar. eg.
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe Ingresar el nombre obligatoriamente");
        }
    }
}
