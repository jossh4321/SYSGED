using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudDenuncia
{
    public class ContenidoSolicitudDenunciaDTOValidator : AbstractValidator<ContenidoSolicitudDenunciaDTO>
    {
        public ContenidoSolicitudDenunciaDTOValidator()
        {
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe ingresar un título obligatoriamente");
            RuleFor(x => x.titulo).Matches("@^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            //RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
        }
    }
}
