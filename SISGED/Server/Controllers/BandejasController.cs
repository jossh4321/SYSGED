using Microsoft.AspNetCore.Mvc;
using SISGED.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BandejasController: ControllerBase
    {
        private readonly BandejaService _bandejaService;

        public BandejasController(BandejaService bandejaService)
        {
            _bandejaService = bandejaService;
        }


    }
}
