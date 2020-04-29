using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Helpers;
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
        private readonly IFileStorage _fileStorage;
        private readonly IMapper _mapper;
        

        public UsuariosController(UsuarioService usuarioservice, 
            RolesService roleservice, IFileStorage fileStorage,
            IMapper mapper)
        {
            _usuarioservice = usuarioservice;
            _roleservice = roleservice;
            _fileStorage = fileStorage;
            _mapper = mapper;
        }
        [HttpGet("todo")]
        public ActionResult<List<Usuario>> Get()
        {
            return _usuarioservice.Get();
        }
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.datos.imagen))
            {
               var profileimg = Convert.FromBase64String(usuario.datos.imagen);
               usuario.datos.imagen = await _fileStorage.saveFile(profileimg,"jpg","usuarios");
            }
            return  _usuarioservice.Post(usuario);
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
        public async Task<ActionResult<Usuario>> Put(Usuario usuario)
        {
            Usuario usuariodb = new Usuario();
            usuariodb = _usuarioservice.GetById(usuario.id);
            string img = usuariodb.datos.imagen;
            usuariodb = _mapper.Map(usuario, usuariodb);
            usuariodb.datos.imagen = img;
            if (!string.IsNullOrWhiteSpace(usuario.datos.imagen))
            {
                var profileimg = Convert.FromBase64String(usuario.datos.imagen);
                usuario.datos.imagen = await _fileStorage.editFile(
                    profileimg, "jpg", "usuarios", usuariodb.datos.imagen);
            }
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
