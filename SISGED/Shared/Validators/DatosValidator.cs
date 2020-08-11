using FluentValidation;
using SISGED.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators
{
    public class DatosValidator : AbstractValidator<Datos>
    {
        public DatosValidator()
        {
            RuleFor(x => x.nombre).NotEmpty()
                .WithMessage("Debe ingresar un nombre");
            RuleFor(x => x.nombre).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                .WithMessage("Debe ingresar un nombre válido").When(x => x.nombre != null && x.nombre != "");
            RuleFor(x => x.apellido).NotEmpty()
                .WithMessage("Debe ingresar un apellido");
            RuleFor(x => x.apellido).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                .WithMessage("Debe ingresar un apellido válido").When(x => x.apellido != null && x.apellido != "");
            RuleFor(x => x.fechanacimiento).NotEmpty()
                .WithMessage("Debe ingresar una fecha de nacimiento");
            RuleFor(x => x.fechanacimiento).Must(BeAValidDate1)
               .WithMessage("El cliente debe ser mayor de 18 años");
            RuleFor(x => x.tipodocumento).NotEmpty()
                .WithMessage("Debe ingresar un tipo de documento");
            RuleFor(x => x.numerodocumento).NotEmpty()
                .WithMessage("Debe ingresar un número de documento");
            RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{8}$").WithMessage("Debe ingresar un número de documento válido").When(x => x.numerodocumento != null && x.numerodocumento != "");
            RuleFor(x => x.direccion).NotEmpty()
                .WithMessage("Debe ingresar una dirección");
            RuleFor(x => x.direccion).Matches(@"^[A-aZ-z0-9ñáéíóú. ]*[A-aZ-z0-9ñáéíóú.]$")
                .WithMessage("Debe ingresar una dirección válida").When(x => x.direccion != null && x.direccion != "");
            RuleFor(x => x.email).NotEmpty()
                .WithMessage("Debe ingresar un email");
            RuleFor(x => x.email)
                .Matches(@"^(([^<>()\[\]\\.,;:\s@”]+(\.[^<>()\[\]\\.,;:\s@”]+)*)|(“.+”))@((\[[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}\.[0–9]{1,3}])|(([a-zA-Z\-0–9]+\.)+[a-zA-Z]{2,}))$")
                .WithMessage("Debe Ingresar un email válido").When(x => x.email != null && x.email != "");
        }
        private bool BeAValidDate1(DateTime date)
        {
            if (date.AddYears(18) > DateTime.Today) { return false; }
            return !date.Equals(default(DateTime));
        }
    }
}
