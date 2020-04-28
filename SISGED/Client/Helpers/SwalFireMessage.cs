using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Client.Helpers
{
    public class SwalFireMessage : ISwalFireMessage
    {
        private readonly IJSRuntime js;

        public SwalFireMessage(IJSRuntime js)
        {
            this.js = js;
        }

        public async Task errorMessage(string mensaje)
        {
            await showMessage("Error", mensaje, "error");
        }

        public async Task successMessage(string mensaje)
        {
            await showMessage("Exitoso", mensaje, "success");
        }

        public async Task infoMessage(string mensaje)
        {
            await showMessage("Atencion", mensaje, "info");
        }

        public async Task warningMessage(string mensaje)
        {
            await showMessage("Cuidado", mensaje, "warning");
        }

        private async ValueTask showMessage(string titulo, string mensaje, string tipoMensaje)
        {
            await js.InvokeVoidAsync("Swal.fire", titulo, mensaje, tipoMensaje);
        }
    }
}
