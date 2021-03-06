﻿using FluentValidation;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators
{
    public class Usuario2Validator: AbstractValidator<UsuarioDTO>
    {
        public Usuario2Validator()
        {
            RuleFor(x => x.usuario).NotEmpty().WithMessage("Debe ingresar un nombre de usuario");
            RuleFor(x => x.clave).NotEmpty().WithMessage("Debe ingresar una clave de usuario");
            //RuleFor(x => x.tipo).NotEmpty().WithMessage("Debe seleccionar un tipo de Usuario");
            RuleFor(x => x.datos).SetValidator(new DatosValidator());
            RuleFor(x => x.rolid).NotEmpty().WithMessage("Debe ingresar un rol");
        }
    }
}
