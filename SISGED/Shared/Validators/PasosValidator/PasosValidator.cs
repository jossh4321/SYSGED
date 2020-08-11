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
            RuleFor(x => x.nombreexpediente).NotEmpty().WithMessage("Debe ingresar un nombre del expediente obligatoriamente");
            RuleForEach(x => x.documentos).SetValidator(new DocumentosPasosValidator());
        }
    }

    public class DocumentosPasosValidator : AbstractValidator<DocumentoPasoDTO2>
    {
        public DocumentosPasosValidator()
        {
            RuleFor(x => x.tipo).NotEmpty().WithMessage("Debe ingresar un nombre del documento obligatoriamente");
            RuleForEach(x => x.pasos).SetValidator(new SubPasosValidator());
        }
    }
    public class SubPasosValidator : AbstractValidator<Paso>
    {
        public SubPasosValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe ingresar un nombre del paso obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripcion del paso obligatoriamente");
            RuleFor(x => x.dias).NotEmpty().WithMessage("Debe ingresar una cantidad de días obligatoriamente");
        }
    }
}
