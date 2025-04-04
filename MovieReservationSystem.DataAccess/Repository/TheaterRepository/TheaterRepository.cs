using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository.TheatorRepository;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.TheaterRepository
{
    public class TheaterRepository : GenericRepository<Theater>, ITheaterRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public TheaterRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
           _applicationDBContext = applicationDBContext;
        }

        public async Task<Theater?> GetTheaterByMovieId(int movieId, bool includeRelated = true)
        {
            if (includeRelated)
            {
                TheatersSchedule? schedule = await _applicationDBContext
                    .TheatersSchedules
                    .FirstOrDefaultAsync(ts => ts.MovieId == movieId);

                if (schedule != null)
                {
                    return await _applicationDBContext
                    .Theaters
                    .Include(t => t.Seats)
                    .Include(t => t.TheatersSchedules)
                    .FirstOrDefaultAsync(t => t.ID == schedule.TheaterId);
                }
                return null;
            }
            else
            {
                TheatersSchedule? schedule = await _applicationDBContext
                    .TheatersSchedules
                    .FirstOrDefaultAsync(ts => ts.MovieId == movieId);

                if (schedule != null)
                {
                    return await _applicationDBContext
                        .Theaters
                        .FirstOrDefaultAsync(t => t.ID == schedule.TheaterId);
                }

                return null;
            }

        }
    }
}
