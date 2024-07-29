using AutoMapper;
using NPWebAPI_Project_1.Models;
using NPWebAPI_Project_1.Models.DTO;

namespace NPWebAPI_Project_1.DtoMapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalParkDto, NationalPark>().ReverseMap();
            CreateMap<Trail,Traildto>().ReverseMap();
        }
    }
}
