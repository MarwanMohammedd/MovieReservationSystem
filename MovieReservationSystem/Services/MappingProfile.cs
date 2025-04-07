using AutoMapper;
using MovieReservationSystem.Model.Models;
using MovieReservationSystem.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MovieReservationSystem.Services
{
    public class MappingProfile : Profile
    {

        public MappingProfile() {

            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<MovieSchedule, MovieScheduleVM>()
                .ForMember(dest => dest.MovieList, opt => opt.Ignore())
               .ForMember(dest => dest.MovieTitle, src => src.MapFrom(src => src.Movie.Title));

            CreateMap< MovieScheduleVM, MovieSchedule>()
               .ForMember(dest => dest.Movie, opt => opt.Ignore());


            CreateMap<Movie, MovieDetailPageVM>().ReverseMap();
        }
    }
}
