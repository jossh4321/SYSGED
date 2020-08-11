﻿using FluentValidation;
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
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar un título obligatoriamente");
            RuleFor(x => x.titulo).Matches("@^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "" && x.titulo.Trim().Length != 0);
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            RuleFor(x => x.nombredenunciante).NotEmpty().WithMessage("Debe ingresar un nombre de denunciante obligatoriamente");
            RuleFor(x => x.lugaraudiencia).NotEmpty().WithMessage("Debe ingresar el lugar de la audiencia obligatoriamente");  
            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario())
                .WithMessage("Debe seleccionar un notario Obligatoriamente");

            RuleFor(x => x.idfiscal).NotEmpty().WithMessage("Debe seleccionar un fiscal obligatoriamente");
            RuleForEach(x => x.participantes).SetValidator(new ParticipanteValidator());
            RuleFor(x => x.participantes)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un participante como mínimo");

            RuleForEach(x => x.hechosimputados).SetValidator(new HechoValidator());
            RuleFor(x => x.hechosimputados)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un hecho inputado como mínimo");



            RuleFor(x => x.fechainicioaudiencia).Must(BeAValidDate1).WithMessage("Fecha de inicio inválida");
            RuleFor(x => x.fechafinaudiencia)
                .Must(BeAValidDate2).WithMessage("Fecha de finalización inválida")
                .GreaterThan(m => m.fechainicioaudiencia)
                            .WithMessage("La fecha fin debe ser después de la fecha inicio");
        }

        private bool BeAValidDate1(DateTime date)
        {
            if (date <= DateTime.UtcNow.AddHours(-5)) { return false; }
            return !date.Equals(default(DateTime));
        }

        private bool BeAValidDate2(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
    public class ParticipanteValidator : AbstractValidator<Participante>
    {
        public ParticipanteValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe ingresar el nombre obligatoriamente");
        }
    }

    public class HechoValidator : AbstractValidator<Hecho>
    {
        public HechoValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar el hecho obligatoriamente");
        }
    }
}
