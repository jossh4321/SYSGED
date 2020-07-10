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
    public class RolesController : ControllerBase
    {
       
        private readonly RolesService _roleservice;
        private readonly IFileStorage _fileStorage;
        private readonly IMapper _mapper;


        public RolesController(RolesService roleservice,
            IFileStorage fileStorage,
            IMapper mapper)
        {
            _roleservice = roleservice;           
            _fileStorage = fileStorage;
            _mapper = mapper;
        }
       
        [HttpGet("id")]
        public ActionResult<Rol> GetById([FromQuery] string id)
        {
            Rol rol = new Rol();
            rol = _roleservice.GetById(id);
            return rol;
        }       
    }
}
