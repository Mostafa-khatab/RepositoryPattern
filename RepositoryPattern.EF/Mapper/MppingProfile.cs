using AutoMapper;
using RepositoryPattern.Api.Controllers;
using RepositoryPattern.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern.EF.Mapper
{
    public class MppingProfile : Profile
    {
        public MppingProfile()
        {
            CreateMap<AuthorDto, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
