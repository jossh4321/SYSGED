using MongoDB.Bson;
using MongoDB.Driver;
using SISGED.Client.Repo;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Services
{
    public class PersonaService
    {
        private readonly IMongoCollection<Persona> _personas;

        public PersonaService(ISysgedDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _personas = database.GetCollection<Persona>("personas");
        }
        //list all documents without filter params
        public List<Persona> Get()
        {
           return  _personas.Find(persona => true).ToList();
        }
        //save a Persona data a return the saved document
        public Persona Post(Persona persona)
        {
            _personas.InsertOne(persona);
            return persona;
        }
        public Persona Put(Persona persona)
        {
            var filter = Builders<Persona>.Filter.Eq("id", persona.id);
            //System.Linq.Expressions.Expression<Func<Persona, bool>> filter2 = persona => persona.Equals(persona.id);

            var update = Builders<Persona>.Update.Set("nombre", "JosueModificado3").Set("apellido","ColomboModificado3");
            
             persona = _personas.FindOneAndUpdate<Persona>(filter,update, new FindOneAndUpdateOptions<Persona>
             {
                 ReturnDocument = ReturnDocument.After
            });
            return persona;
        }

    }
}
