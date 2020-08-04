using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.EntregaExpedienteNotario
{
    public class EntregaExpedienteNotarioValidator : AbstractValidator<EntregaExpedienteNotarioDTO>
    {
        public EntregaExpedienteNotarioValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoEntregaExpedienteNotarioValidator());
        }
    }
}
