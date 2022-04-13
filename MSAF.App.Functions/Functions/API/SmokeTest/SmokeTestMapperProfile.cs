using AutoMapper;
using MSAF.App.DAL.SmokeTest;

namespace MSAF.App.Functions.SmokeTest
{
    public class SmokeTestMapperProfile : Profile
    {
        public SmokeTestMapperProfile()
        {
            CreateMap<SmokeTestData, SmokeTestResponseModel>()
                .ForMember(dest => dest.IsMapperOK, opt => opt.MapFrom(src => true))
                .ReverseMap();
        }
    }
}
