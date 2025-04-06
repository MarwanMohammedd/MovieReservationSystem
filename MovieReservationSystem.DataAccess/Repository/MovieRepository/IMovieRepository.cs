using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.MovieRepository
{
    public interface IMovieRepository:IGenericRepository<Movie>
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(bool includeRelated = true);
        Task<Movie?> GetMovieByIdAsync(int id, bool includeRelated = true);
        Task<Movie?> GetMovieByTitleAsync(string title, bool includeRelated = true);
 

        //Task AddMovieAsync(Movie movie);
        //Task<bool> UpdateMovieAsync(Movie movie);
        //Task<bool> DeleteMovieAsync(int id);
        //Task<IEnumerable<Movie>> GetUpcomingMoviesAsync();
        //Task<IEnumerable<Movie>> GetNowShowingMoviesAsync();
    }
}
