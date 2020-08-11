using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Validators.FiltroEscrituraPublicaValidator;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.OficioBPN
{
    public class ContenidoOficioBPNValidator : AbstractValidator<ContenidoOficioBPNDTO>
    {
        public ContenidoOficioBPNValidator(){
            RuleFor(x => x.titulo).NotEmpty().WithMessage("Debe Ingresar un titulo obligatoriamente");
            RuleFor(x => x.titulo).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                                             .WithMessage("Debe ingresar un título válido").When(x => x.titulo != null && x.titulo != "" && x.titulo.Trim().Length != 0);
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripción obligatoriamente");
            RuleFor(x => x.direccionoficio).NotEmpty().WithMessage("Debe ingresar una dirección de oficio obligatoriamente");
            RuleFor(x => x.actojuridico).NotEmpty().WithMessage("Debe ingresar un acto jurídico obligatoriamente");
            RuleFor(x => x.tipoprotocolo).NotEmpty().WithMessage("Debe ingresar un tipo de protocolo obligatoriamente");
            //RuleFor(x => x.data).NotEmpty().WithMessage("Debe Ingresar un archivo obligatoriamente, en el primer tab");
            RuleFor(x => x.otorgantes).Must(x => x.Count >= 1).WithMessage("Debe agregar un otorgante como mínimo");

            RuleFor(x => x.idnotario).Must(notario => notario != null && notario != new Notario())
                .WithMessage("Debe seleccionar un notario obligatoriamente");
            RuleFor(x => x.idcliente).Must(cliente => cliente != null && cliente != new Usuario())
                .WithMessage("Debe seleccionar un usuario obligatoriamente");
        }
    }
}
