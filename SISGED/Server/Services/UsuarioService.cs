using MongoDB.Bson;
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
                .Set("rol", usuario.rol);
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

        public Usuario modifyPassword(Usuario usuario)
        {
            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);            
            var update = Builders<Usuario>.Update
                .Set("clave", usuario.clave);
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

        public async Task<List<UsuarioRDTO>> obtenerFiscales()
        {
            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idrol") }
                        )
                    ))
                );
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "roles")
                .Add("let", new BsonDocument("idrol", "$rol"))
                .Add("pipeline", subpipeline)
                .Add("as", "rolobj"));

            var filter2 = new BsonDocument("$match",
                new BsonDocument("rolobj.label","Fiscal"));
            var filter1 = new BsonDocument("$match",
                new BsonDocument("tipo", "administracion"));

            List<UsuarioRDTO> fiscales = new List<UsuarioRDTO>();
            fiscales = await _usuarios.Aggregate()
                .AppendStage<Usuario>(filter1)
                .AppendStage<Usuario_LK>(lookup)
                .Unwind<Usuario_LK, UsuarioRDTO>(p => p.rolobj)
                .AppendStage<UsuarioRDTO>(filter2)
                .ToListAsync();
            return fiscales;
        }

        public List<Usuario> filter(string term)
        {
            string regex = "\\b" + term.ToLower() + ".*";
            var filter = Builders<Usuario>.Filter.Regex("datos.nombre", new BsonRegularExpression(regex, "i"));
            var filter2 = Builders<Usuario>.Filter.Eq("tipo", "cliente");
            return _usuarios.Find(filter & filter2).ToList();
        }


        public Usuario modifyDatos(Usuario usuario)
        {
            var filter = Builders<Usuario>.Filter.Eq("id", usuario.id);
            string newusuario = usuario.usuario ;
            string newnombre = usuario.datos.nombre;
            string newapellido = usuario.datos.apellido;
            DateTime newfecha = usuario.datos.fechanacimiento;
            string newtipodoc = usuario.datos.tipodocumento;
            string newnumdoc = usuario.datos.numerodocumento;
            string newdireccion = usuario.datos.direccion;
            string newemail = usuario.datos.email;
            string newimage = usuario.datos.imagen;
            var update = Builders<Usuario>.Update
                .Set("usuario", newusuario)
                .Set("datos.nombre", newnombre)
                .Set("datos.apellido", newapellido)
                .Set("datos.fechanacimiento", newfecha)
                .Set("datos.tipodocumento", newtipodoc)
                .Set("datos.numerodocumento", newnumdoc)
                .Set("datos.direccion", newdireccion)
                .Set("datos.email", newemail)
                .Set("datos.imagen", newimage);
            usuario = _usuarios.FindOneAndUpdate<Usuario>(filter, update, new FindOneAndUpdateOptions<Usuario>
            {
                ReturnDocument = ReturnDocument.After
            });
            return usuario;
        }
        /*
         public List<UsuarioRDTO> Get()
        {
            //List<Usuario> usuarios = new List<Usuario>();
            //usuarios = _usuarios.Find(usuario => true).ToList();
            //
            BsonArray subpipeline = new BsonArray();
            subpipeline.Add(
                new BsonDocument("$match", new BsonDocument(
                    "$expr", new BsonDocument(
                        "$eq", new BsonArray { "$_id", new BsonDocument("$toObjectId", "$$idrol") }
                        )
                    ))
                );
            var lookup = new BsonDocument("$lookup",
                new BsonDocument("from", "roles")
                .Add("let", new BsonDocument("idrol", "$rol"))
                .Add("pipeline", subpipeline)
                .Add("as", "rolobj"));

            List<UsuarioRDTO> usuariosF = new List<UsuarioRDTO>();
            usuariosF = _usuarios.Aggregate()
                .AppendStage<List<UsuarioRDTO>>(lookup)
                .First();

            return usuariosF;
        }
         */

        public List<Usuario> filtroEvaluar(string term)
        {
            string regex = "\\b" + term.ToLower() + ".*";
            var filter = Builders<Usuario>.Filter.Regex("datos.numerodocumento", new BsonRegularExpression(regex, "i"));
            var filter2 = Builders<Usuario>.Filter.Eq("tipo", "administracion");
            return _usuarios.Find(filter & filter2).ToList();
        }


        public async Task<List<usuario_unwind>> obtenerUsuariosAutoComplete(string val)
        {

            var project1 = new BsonDocument("$project",
                                new BsonDocument
                                    {
                                        { "_id", "$_id" },
                                        { "tipo", "$tipo" },
                                        { "datos", "$datos" },
                                        { "rol", "$rol" },
                                        { "nombre",
                                new BsonDocument("$toLower", "$datos.nombre") }
                                    });
            var match = new BsonDocument("$match",
                            new BsonDocument
                                {
                                    { "tipo", "administracion" },
                                    { "nombre",
                            new BsonDocument("$regex", val) }
                                });
            var lookup = new BsonDocument("$lookup",
                                new BsonDocument
                                    {
                                        { "from", "roles" },
                                        { "let",
                                new BsonDocument("idrol", "$rol") },
                                        { "pipeline",
                                new BsonArray
                                        {
                                            new BsonDocument("$match",
                                            new BsonDocument("$expr",
                                            new BsonDocument("$and",
                                            new BsonArray
                                                        {
                                                            new BsonDocument("$eq",
                                                            new BsonArray
                                                                {
                                                                    "$_id",
                                                                    new BsonDocument("$toObjectId", "$$idrol")
                                                                })
                                                        })))
                                        } },
                                        { "as", "rolobj" }
                                    });

            List<usuario_unwind> usuarios = new List<usuario_unwind>();
            usuarios = await _usuarios.Aggregate()
                .AppendStage<usuario_project1>(project1)
                .AppendStage<usuario_project1>(match)
                .AppendStage<usuario_lookup1>(lookup)
                .Unwind<usuario_lookup1, usuario_unwind>(p => p.rolobj)
                .ToListAsync();
            return usuarios;
        }

    }
}
