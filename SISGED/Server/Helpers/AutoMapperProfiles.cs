using AutoMapper;
using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, Usuario>()
                .ForMember(x => x.estado, option => option.Ignore());
        }
    }
}
