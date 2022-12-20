using AutoMapper;
using Books.API.Entities;
using Books.API.Models.Dto;
using System.Linq;

namespace Books.API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            CreateMap<LoginRequestDto, LocalUser>();

            CreateMap<(ApplicationUser LocalUser, string Token), LoginResponseDto>()
                  .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.LocalUser))
                  .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token));

            CreateMap<LoginRequestDto, ApplicationUser>();

            CreateMap<RegisterationRequestDto, ApplicationUser>()
                 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));

            CreateMap<ApplicationUser, RegisterationResponsetDto>();

            CreateMap<ApplicationUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain == true).Url));

            CreateMap<MemberUpdateDto, ApplicationUser>();
        }
    }
}
