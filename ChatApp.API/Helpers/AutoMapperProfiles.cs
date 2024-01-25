using AutoMapper;
using ChatApp.API.DTOs;
using ChatApp.API.Entities;
using ChatApp.API.Extensions;

namespace ChatApp.API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, MemberDto>()
            .ForMember(dest => dest.Age, options =>
                        options.MapFrom(source => source.DateOfBirth.CalculateAge()))
            .ForMember(s => s.ImageUrl, options =>
                        options.MapFrom(source => source.Photos!.FirstOrDefault(p => p.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
    }
}
