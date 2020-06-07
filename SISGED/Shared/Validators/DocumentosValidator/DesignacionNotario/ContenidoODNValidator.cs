﻿using FluentValidation;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class ContenidoODNValidator : AbstractValidator<ContenidoOficioDesignacionNotario>
    {
        public ContenidoODNValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            RuleFor(x => x.lugaroficionotarial).NotEmpty().WithMessage("Debe Ingresar un lugar obligatoriamente");
        }
    }
}