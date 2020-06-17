using MongoDB.Driver;
using SISGED.Server.Helpers;
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

            _usuarios = database.GetCollection<Usuario>("usuarios");
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
        public Usuario GetById(string id)
        {
            Usuario usuario = new Usuario();
            usuario = _usuarios.Find(usuario => usuario.id == id).FirstOrDefault();
            return usuario;
        }

        public Usuario Post(Usuario usuario)
        { 
            _usuarios.InsertOne(usuario);
            return usuario;
        }

        public Usuario GetByUserNameAndPass(UserInfo userinfo)
        {
            var result = _usuarios.Find(usuarios => usuarios.usuario == userinfo.usuario && usuarios.clave == userinfo.clave).FirstOrDefault();
            return result;
        }
        public Usuario Put(Usuario usuario)
        {
            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);
            var update = Builders<Usuario>.Update
                .Set("usuario", usuario.usuario)
                .Set("clave", usuario.clave)
                .Set("datos", usuario.datos)
                .Set("roles", usuario.rol);
            usuario = _usuarios.FindOneAndUpdate<Usuario>(filter, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });
            return usuario;
        }
        public Usuario modifyState(Usuario usuario)
        {
            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);
            string newState = usuario.estado == "activo" ? "inactivo" : "activo";
            var update = Builders<Usuario>.Update
                .Set("estado", newState);
            usuario = _usuarios.FindOneAndUpdate<Usuario>(filter, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });
            return usuario;
        }
        public List<Usuario> GetByStatus(string status)
        {
            List<Usuario> usuarios = new List<Usuario>();
            usuarios = _usuarios.Find(usuario => usuario.estado == status).ToList();
            return usuarios;
        }
    }
}
