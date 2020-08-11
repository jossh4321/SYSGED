using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class CalendarEvent
    {
        public DateTime fechaDelEvento { get; set; }
        public string nombreCliente { get; set; }
        public string nombreEmisor { get; set; }
        public string tipoDocumento { get; set; }
    }
}
