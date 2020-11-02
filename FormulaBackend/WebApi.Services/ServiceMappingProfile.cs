using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi.DataAccess.Models;
using WebApi.Services.DTOs;

namespace WebApi.Services
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<TeamDTO, Team>();
            CreateMap<Team, TeamDTO>();
            CreateMap<UserDTO, ApplicationUser>();
            CreateMap<ApplicationUser, UserDTO>();
        }
    }
}
