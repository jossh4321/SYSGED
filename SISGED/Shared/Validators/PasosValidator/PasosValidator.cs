using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.PasosValidator
{
    public class PasosValidator : AbstractValidator<PasosDTO2>
    {
        public PasosValidator()
        {
            RuleFor(x => x.nombreexpediente).NotEmpty().WithMessage("Debe Ingresar un Nombre del expediente obligatoriamente");
            RuleForEach(x => x.documentos).SetValidator(new DocumentosPasosValidator());
        }
    }

    public class DocumentosPasosValidator : AbstractValidator<DocumentoPasoDTO2>
    {
        public DocumentosPasosValidator()
        {
            RuleFor(x => x.tipo).NotEmpty().WithMessage("Debe Ingresar un Nombre del Documento obligatoriamente");
            RuleForEach(x => x.pasos).SetValidator(new DocumentosSubPasosValidator());
        }
    }
    public class DocumentosSubPasosValidator : AbstractValidator<Paso>
    {
        public DocumentosSubPasosValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe Ingresar un Nombre del Paso obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una Descripcion del Paso obligatoriamente");
            RuleFor(x => x.dias).NotEmpty().WithMessage("Debe Ingresar una cantidad de Dias Obligatoriamente");
            RuleForEach(x => x.subpaso).SetValidator(new SubSubPasosValidator());
        }
    }

    public class SubSubPasosValidator: AbstractValidator<SubPaso>
    {
        public SubSubPasosValidator()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe Ingresar una Descripcion del Sub Paso obligatoriamente");
        }

    }
}
