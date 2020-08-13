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
            RuleFor(x => x.pasos)
           .Must(x => x.Count >= 1).WithMessage("Debe agregar un paso como mínimo");
        }
    }
    public class SubPasosValidator : AbstractValidator<PasoDocDTO>
    {
        public SubPasosValidator()
        {
            RuleFor(x => x.nombre).NotEmpty().WithMessage("Debe ingresar un nombre del paso obligatoriamente");
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripcion del paso obligatoriamente");
            RuleFor(x => x.dias).NotEmpty().WithMessage("Debe ingresar una cantidad de días obligatoriamente");
            RuleForEach(x => x.subpaso).SetValidator(new InformacionSubpaso());
            RuleFor(x => x.subpaso)
            .Must(x => x.Count >= 1).WithMessage("Debe agregar un sub-paso como mínimo");
        }
    }

    public class InformacionSubpaso : AbstractValidator<SubPaso>
    {
        public InformacionSubpaso()
        {
            RuleFor(x => x.descripcion).NotEmpty().WithMessage("Debe ingresar una descripcion obligatoriamente");
        }
    }
}
