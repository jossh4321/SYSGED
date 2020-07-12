using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Validators.DocumentosValidator.ResultadoBPN
{
    public class ContenidoResultadoBPNValidator : AbstractValidator<ContenidoResultadoBPNDTO>
    {
        public ContenidoResultadoBPNValidator()
        {
            //DOCUMENTO OBLIGATORIO
            RuleFor(x => x.idescriturapublica).Must(escritura => escritura != null && escritura != new EscrituraPublica())
                .WithMessage("Debe seleccionar una escritura pública obligatoriamente");
            RuleFor(x => x.costo).Must(BeAValidCosto).WithMessage("El costo debe ser mayor a 0");
            RuleFor(x => x.cantidadfoja).Must(BeAValidCantidadFojas).WithMessage("La cantidad de fojas debe ser mayor a 0");
        }

        private bool BeAValidCosto(Int32 costo)
        {
            if (costo <= 0) { return false; }
            else { return true; }
        }

        private bool BeAValidCantidadFojas(Int32 cantidadFoja)
        {
            if (cantidadFoja <= 0) { return false; }
            else { return true; }
        }
    }
}
