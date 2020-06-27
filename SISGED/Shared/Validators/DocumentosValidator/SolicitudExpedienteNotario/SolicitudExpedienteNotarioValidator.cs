using FluentValidation;
using SISGED.Shared.DTOs;
using SISGED.Shared.Validators.DocumentosValidator.SolicitudExpedienteNotario;

namespace SISGED.Shared.Validators.DocumentosValidator.SolicitudExpedicionNotario
{
    public class SolicitudExpedienteNotarioValidator : AbstractValidator<SolicitudExpedienteNotarioDTO>
    {
        public SolicitudExpedienteNotarioValidator()
        {
            RuleFor(x => x.contenidoDTO).SetValidator(new ContenidoSolicitudExpedienteNotarioValidator());
        }
    }
}
