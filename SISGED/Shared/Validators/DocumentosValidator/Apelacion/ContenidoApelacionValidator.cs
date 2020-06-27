using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.Apelacion
{
    public class ContenidoApelacionValidator : AbstractValidator<ContenidoApelacionDTO>
    {
        public ContenidoApelacionValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una descripcion obligatoriamente");
            //DOCUMENTO OBLIGATORIO
        }
    }
}
