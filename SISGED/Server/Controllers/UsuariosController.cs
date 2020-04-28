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
        private readonly RolesService _roleservice;

        public UsuariosController(UsuarioService usuarioservice, RolesService roleservice)
        {
            _usuarioservice = usuarioservice;
            _roleservice = roleservice;
        }
        [HttpGet("todo")]
        public ActionResult<List<Usuario>> Get()
        {
            return _usuarioservice.Get();
        }
        [HttpPost]
        public ActionResult<Usuario> Post(Usuario usuario)
        {
            return _usuarioservice.Post(usuario);
        }
        [HttpGet("roles")]
        public ActionResult<List<Rol>> GetRoles()
        {
            List<Rol> listaroles = new List<Rol>();
            listaroles = _roleservice.Get();
            return listaroles;
        }
        [HttpGet("id")]
        public ActionResult<Usuario> GetById([FromQuery] string id)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarioservice.GetById(id);
            return usuario;
        }
        [HttpPut]
        public ActionResult<Usuario> Put(Usuario usuario)
        {
            usuario = _usuarioservice.Put(usuario);
            return usuario;
        }

        [HttpPut("estado")]
        public ActionResult<Usuario> ModifyState(Usuario usuario)
        {
            usuario = _usuarioservice.modifyState(usuario);
            return usuario;
        }

    }
}
