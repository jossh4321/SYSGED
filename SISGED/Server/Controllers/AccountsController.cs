using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
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
        private readonly IConfiguration _configuration;
        public AccountsController(UsuarioService usuarioservice,
            IConfiguration configuration)
        {
            _usuarioservice = usuarioservice;
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
                return BuildToken(model, new List<string>());
            }
            else
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserToken>> Login(/*[FromBody]*/ UserInfo userInfo)
        {
            var result =  _usuarioservice.GetByUserNameAndPass(userInfo);
            if (result != null)
            {
                Usuario usuario = _usuarioservice.GetByUsername(userInfo.usuario);
                var roles = usuario.roles.Select(x => x.nombre).ToList();

                return BuildToken(userInfo, roles);
            }
            else
            {
                return BadRequest("Invalid login attempt");
            }
        }
        private UserToken BuildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.usuario),
                new Claim(ClaimTypes.Name, userInfo.usuario),
                new Claim("miValor", "Lo que yo quiera"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }


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
