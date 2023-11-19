using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Presentation.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Presentation.Mapping
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<User, ApplicationUserIResultModel>()
                .ReverseMap();           
        }
    }
}
