using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.MovieRepository
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDBContext _context;

        public MovieRepository(ApplicationDBContext context):base(context) 
        {
            _context = context;
        }


        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(bool includeRelated = true)
        {
            if (includeRelated)
            {
                return await _context.Movies
                    .Include(m => m.MovieSchedule)
                    .Include(m => m.Reviews)
                    .Include(m => m.Tickets)
                    .Include(m => m.TheatersSchedules)
                    .ToListAsync();
            }
            return await GetAllAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id, bool includeRelated = true)
        {
            if (includeRelated)
            {
                return await _context.Movies
                    .Include(m => m.MovieSchedule)
                    .Include(m => m.Reviews)
                    .Include(m => m.Tickets)
                    .Include(m => m.TheatersSchedules)
                    .FirstOrDefaultAsync(m => m.ID == id);
            }
            return await GetItemAsync(m => m.ID == id);
        }

        public async Task<Movie?> GetMovieByTitleAsync(string title, bool includeRelated = true)
        {
            if (includeRelated)
            {
                return await _context.Movies
                    .Include(m => m.MovieSchedule)
                    .Include(m => m.Reviews)
                    .Include(m => m.Tickets)
                    .Include(m => m.TheatersSchedules)
                    .FirstOrDefaultAsync(m => m.Title == title);
            }
            return await GetItemAsync(m => m.Title == title);
        }


        public async Task<IEnumerable<Movie>> GetUpcomingMoviesAsync()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Movies
                .Where(m => m.ProductionYear > currentDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Movie>> GetNowShowingMoviesAsync()
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Movies
                .Where(m => m.ProductionYear <= currentDate)
                .ToListAsync();
        }
      
    }
}
