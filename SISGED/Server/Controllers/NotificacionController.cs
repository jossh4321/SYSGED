using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        [HttpGet("notificacion")]
        public ActionResult<List<NotificacionDTO>> getnotificaciones()
        {
            return _notificacion.obtenernotificacion();
        }
    }
}
