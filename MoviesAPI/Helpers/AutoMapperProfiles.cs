using AutoMapper;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ActorDTO,Actor>().ReverseMap();
            CreateMap<ActorCreationDTO,Actor>().ReverseMap();
            CreateMap<Genre,GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO,Genre>();
        }
    }
}
