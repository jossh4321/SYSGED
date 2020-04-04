using MongoDB.Driver;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;
        public UsuarioService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _usuarios = database.GetCollection<Usuario>("usuario");
        }

        public List<Usuario> Get()
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = _usuarios.Find(usuario => true).ToList();
            return usuarios;
        }

        public Usuario GetByUsername(string username)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarios.Find(usuario => usuario.usuario == username).FirstOrDefault();
            return usuario;
        }
        public Usuario Post(Usuario usuario)
        {
            _usuarios.InsertOne(usuario);
            return usuario;
        }
        public Usuario GetByUserNameAndPass(UserInfo userinfo)
        {
            var result = _usuarios.Find(usuario => usuario.usuario == userinfo.usuario && usuario.clave == userinfo.clave).FirstOrDefault();
            return result;
        }
    }
}
