using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using SISGED.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController: ControllerBase
    {
        private readonly UsuarioService _usuarioservice;
        private readonly RolesService _rolservice;
        private readonly PermisosService _permisoservice;
        private readonly IConfiguration _configuration;
        public AccountsController(UsuarioService usuarioservice, RolesService rolservice,
            PermisosService permisoservice,
            IConfiguration configuration)
        {
            _rolservice = rolservice;
            _usuarioservice = usuarioservice;
            _permisoservice = permisoservice;
            _configuration = configuration;
        }
        [HttpPost("crear")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> CreateUser(
            /*[FromBody]*/ UserInfo model)
        {
            //creando Usuario con el Identity
            Console.WriteLine("Hey Llegue!");
            var user = new Usuario()
            { usuario = model.usuario, clave = model.clave };
            var result = _usuarioservice.Post(user);
            if (result != null)
            {
                return BuildToken(model, "");
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpGet("GetUserData")]
        [AllowAnonymous]
        public ActionResult<Sesion> GetDatosUsuario([FromQuery] string user)
        {
            Sesion temp = new Sesion();
            List<Permiso> permisosInterfaces = new List<Permiso>();
            List<Permiso> permisosHerramientas = new List<Permiso>();

            Usuario usuario = new Usuario();
            usuario = _usuarioservice.GetByUsername(user);

            Rol rolusu = new Rol();
            rolusu = _rolservice.GetById(usuario.rol);

            foreach (string idPerm in rolusu.listainterfaces)
            {
                //Obtener el bjeto permiso y añadirlo
                Permiso perm1 = new Permiso();
                perm1 = _permisoservice.GetById(idPerm);
                permisosInterfaces.Add(perm1);
            }

            foreach (string idPerm in rolusu.listaherramientas)
            {
                //Obtener el bjeto permiso y añadirlo
                Permiso perm2 = new Permiso();
                perm2 = _permisoservice.GetById(idPerm);
                permisosHerramientas.Add(perm2);
            }            

            temp.nombre = usuario.datos.nombre;
            temp.rol = rolusu.nombre;
            temp.permisosHerram = permisosHerramientas;
            temp.permisosInterfaz = permisosInterfaces;
            return temp;
        }

        [HttpGet("GetRolByID")]
        [AllowAnonymous]
        public ActionResult<Rol> GetRolByID([FromQuery] string id)
        {
            Rol rolusu = new Rol();
            rolusu = _rolservice.GetById(id);
            return rolusu;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login(/*[FromBody]*/ UserInfo userInfo)
        {
            var result =  _usuarioservice.GetByUserNameAndPass(userInfo);
            if (result != null)
            {
                Usuario usuario = _usuarioservice.GetByUsername(userInfo.usuario);
                Rol rolusu = _rolservice.GetById(usuario.rol);
                //var roles = usuario.roles.Select(x => x.nombre).ToList();
                //List<String> roles = new List<String>(){ "admin" };
                return BuildToken(userInfo, rolusu.nombre);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }
        private UserToken BuildToken(UserInfo userInfo, String rol)//IList<string> roles
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.usuario),
                new Claim(ClaimTypes.Name, userInfo.usuario),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            /*foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }*/
            claims.Add(new Claim(ClaimTypes.Role, rol));

            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, 
                SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
