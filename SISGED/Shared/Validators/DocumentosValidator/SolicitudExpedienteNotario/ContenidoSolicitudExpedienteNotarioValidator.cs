﻿using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudExpedienteNotario
{
    public class ContenidoSolicitudExpedienteNotarioValidator : AbstractValidator<ContenidoSolicitudExpedienteNotarioDTO>
    {
        public ContenidoSolicitudExpedienteNotarioValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
        }
    }
}