using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Api.ViewModels;
using WebApi.Services.DTOs;

namespace WebApi.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TeamViewModel, TeamDTO>();
            CreateMap<TeamDTO, TeamViewModel>();
            CreateMap<CreateTeamViewModel, TeamDTO>();
            CreateMap<UpdateTeamViewModel, TeamDTO>()
                .IncludeMembers(s => s.Updated);
        }
    }

}
