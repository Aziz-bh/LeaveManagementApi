using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
    }
}