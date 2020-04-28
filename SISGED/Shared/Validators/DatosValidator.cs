using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators
{
    public class DatosValidator: AbstractValidator<Datos>
    {
        public DatosValidator()
        {
            RuleFor(x => x.nombre).NotEmpty()
                .WithMessage("Debe Ingresar un Nombre");
            RuleFor(x => x.apellido).NotEmpty()
                .WithMessage("Debe Ingresar un Apellido");
            RuleFor(x => x.fechanacimiento).NotEmpty()
                .WithMessage("Debe Ingresar una Fecha de Nacimiento");
            RuleFor(x => x.tipodocumento).NotEmpty()
                .WithMessage("Debe Ingresar un Tipo de Doc.");
            RuleFor(x => x.numerodocumento).NotEmpty()
                .WithMessage("Debe Ingresar un Numero de Doc.");
            RuleFor(x => x.direccion).NotEmpty()
                .WithMessage("Debe Ingresar una Direccion");
            RuleFor(x => x.email).NotEmpty()
                .WithMessage("Debe Ingresar un email");
        }
    }
}
