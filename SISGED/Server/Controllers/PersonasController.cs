using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Helpers;
using SISGED.Server.Services;
using SISGED.Shared.DTOs;
using SISGED.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SISGED.Server.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults
             .AuthenticationScheme)]
    public class PersonasController: ControllerBase
    {
        private readonly PersonaService _personaservice;

        public PersonasController(PersonaService personasevice)
        {
            _personaservice = personasevice;
        }
        //non paginated
        [HttpGet]
        public ActionResult<List<Persona>> Get()
        {
            return _personaservice.Get();
        }
        //paginated
        [HttpGet("paginated")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Persona>>> GetPaginated([FromQuery] Pagination pagination)
        {
            var queryable = _personaservice.Get().AsQueryable();
            await HttpContext.InsertPagedParameterOnResponse(queryable, pagination.QuantityPerPage);
            return  queryable.Paginate(pagination).ToList();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<Persona> Post(Persona persona)
        {
            persona = _personaservice.Post(persona);
            return persona;
        }
        [HttpPut]
        public ActionResult<Persona> Put(Persona persona)
        {
            //5e8236cba608e13a741aa23c
            persona = _personaservice.Put(persona);
            return persona;
        }

    }
}
