using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Sheep, SheepDto>();
            CreateMap<SheepDto, Sheep>();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, GetOrderDtos>().ReverseMap();
        }
    }
}