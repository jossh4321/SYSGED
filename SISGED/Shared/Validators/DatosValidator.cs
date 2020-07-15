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
            RuleFor(x => x.nombre).Matches(@"^[A-aZ-z0-9áéíóú. ]*[A-aZ-z0-9áéíóú.]$")
                .WithMessage("Debe ingresar un nombre válido").When(x => x.nombre != null && x.nombre != "");
            RuleFor(x => x.apellido).NotEmpty()
                .WithMessage("Debe Ingresar un Apellido");
            RuleFor(x => x.apellido).Matches(@"^[A-aZ-z0-9áéíóú. ]*[A-aZ-z0-9áéíóú.]$")
                .WithMessage("Debe ingresar un apellido válido").When(x => x.apellido != null && x.apellido != "");
            RuleFor(x => x.fechanacimiento).NotEmpty()
                .WithMessage("Debe Ingresar una Fecha de Nacimiento");
            RuleFor(x => x.tipodocumento).NotEmpty()
                .WithMessage("Debe Ingresar un Tipo de Doc.");
            RuleFor(x => x.numerodocumento).NotEmpty()
                .WithMessage("Debe ingresar un número de documento");
            RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{8}$").WithMessage("Debe ingresar un número de doc válido").When(x => x.numerodocumento != null && x.numerodocumento != "");
            RuleFor(x => x.direccion).NotEmpty()
                .WithMessage("Debe Ingresar una Direccion");
            RuleFor(x => x.direccion).Matches(@"^[A-aZ-z0-9áéíóú. ]*[A-aZ-z0-9áéíóú.]$")
                .WithMessage("Debe ingresar una dirección válida").When(x => x.direccion != null && x.direccion != "");
            RuleFor(x => x.email).NotEmpty()
                .WithMessage("Debe Ingresar un email");
            RuleFor(x => x.email)
                .Matches(@"^(([^<>()\[\]\\.,;:\s@”]+(\.[^<>()\[\]\\.,;:\s@”]+)*)|(“.+”))@((\[[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}])|(([a-zA-Z\-0–9]+\.)+[a-zA-Z]{2,}))$")
                .WithMessage("Debe Ingresar un email válido").When(x => x.email != null && x.email != "");
        }
    }
}
