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
    public class UsuariosController: ControllerBase
    {
        private readonly UsuarioService _usuarioservice;

        public UsuariosController(UsuarioService usuarioservice)
        {
            _usuarioservice = usuarioservice;
        }
        [HttpGet]
        public ActionResult<List<Usuario>> Get()
        {
            return _usuarioservice.Get();
        }
        
    }
}
