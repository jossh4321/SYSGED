using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.EntregaExpedienteNotario
{
    public class ContenidoEntregaExpedienteNotarioValidator : AbstractValidator<ContenidoEntregaExpedienteNotarioDTO>
    {
        public ContenidoEntregaExpedienteNotarioValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.titulo).Matches("@^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "" && x.titulo.Trim().Length != 0);
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario()).WithMessage("Debe seleccionar un notario obligatoriamente");
        }
    }
}
