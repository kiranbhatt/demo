using AutoMapper;
using Books.API.Entities;
using Books.API.Models.Dto;

namespace Books.API.Profiles
{
    public class PhotosProfile : Profile
    {
        public PhotosProfile()
        {
            CreateMap<Photo, PhotoDto>().ReverseMap();

        }
    }
}
