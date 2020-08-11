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
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar un título obligatoriamente");
            RuleFor(x => x.titulo).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "" && x.titulo.Trim().Length != 0);
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            //DOCUMENTO OBLIGATORIO
        }
    }
}
