using FluentValidation;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.UsuarioValidator
{
    public class UsuarioValidator: AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(x => x.usuario).NotEmpty().WithMessage("Debe ingresar un nombre de usuario");
            RuleFor(x => x.datos).SetValidator(new DatosValidator());
        }
    }
}
