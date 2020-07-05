using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    public class NotificacionController : ControllerBase
    {
        private readonly NotificacionService _notificacion;

        public NotificacionController(NotificacionService notificacion)
        {
            _notificacion = notificacion;
        }

        [HttpGet]
        public ActionResult<List<Notificacion>> Get()
        {
            return _notificacion.Get();
        }
    }
}
