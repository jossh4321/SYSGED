using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SISGED.Shared.Validators.DocumentosValidator.DesignacionNotario
{
    public class SolicitudExpedicionFirmaValidator : AbstractValidator<SolicitudExpedicionFirmaDTO>
    {
        public SolicitudExpedicionFirmaValidator()
        {
            //RuleFor(x => x.nombrecliente).NotEmpty().WithMessage("Debe Ingresar un cliente obligatoriamente");
            //RuleFor(x => x.tipodocumento).NotEmpty().WithMessage("Debe Ingresar un tipo de documento obligatoriamente");
            //RuleFor(x => x.numerodocumento).NotEmpty().WithMessage(" ");
            //RuleFor(x => x.numerodocumento).Matches(@"^[0]{999}$").WithMessage("Primero Seleccione un numero documento").When(x => x.tipodocumento == null || x.tipodocumento == "");
            //RuleFor(x => x.numerodocumento).Matches(@"^[0]{999}$").WithMessage("Primero Seleccione un numero documento").When(x => x.tipodocumento == "nulo");
            //RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{9}$").WithMessage("El CE requiere de 9 digitos numericos").When(x => x.tipodocumento == "CE");
            //RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{8}$").WithMessage("El DNI requiere de 8 digitos numericos").When(x => x.tipodocumento == "DNI");
            //RuleFor(x => x.numerodocumento).Matches(@"^[0-9]{7}$").WithMessage("El Pasaporte requiere de 7 digitos").When(x => x.tipodocumento == "pasaporte");
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSEFValidator());
        }
    }
}
