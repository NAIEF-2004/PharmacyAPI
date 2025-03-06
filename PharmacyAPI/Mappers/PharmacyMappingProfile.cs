using AutoMapper;
using PharmacyAPI.Models;
using PharmacyAPI.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PharmacyAPI.Mappers
{
    public class PharmacyMappingProfile : Profile
    {
        public PharmacyMappingProfile()
        {
            CreateMap<Pharmacist, PharmacistDTO>().ReverseMap();
        }
    }
}
