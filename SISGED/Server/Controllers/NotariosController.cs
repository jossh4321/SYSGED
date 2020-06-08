using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Services;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotariosController : ControllerBase
    {
        private readonly NotarioService _notarioService;

        public NotariosController(NotarioService notarioService)
        {
            _notarioService = notarioService;
        }

        [HttpGet("filter")]
        public ActionResult<List<Notario>> autocompletefilter([FromQuery] string term)
        {
            List<Notario> listanotario = new List<Notario>();
            listanotario =  _notarioService.filter(term);
            return listanotario;
        }

        /*[HttpGet("id")]
        public ActionResult<Usuario> GetById([FromQuery] string id)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarioservice.GetById(id);
            return usuario;
        }*/
    }
}
