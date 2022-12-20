using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Books.API.Profiles
{
    public class BooksProfile : Profile
    {
        public BooksProfile()
        {
            CreateMap<Entities.Book, Book>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src =>
                    $"{src.Author.FirstName} {src.Author.LastName}"));

            CreateMap<BookForCreation, Entities.Book>();


            CreateMap<JsonPatchDocument<BookForCreation>, JsonPatchDocument<Book>>();
        }
    }
}
