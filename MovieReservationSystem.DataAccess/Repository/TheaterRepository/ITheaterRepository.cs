using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.TheatorRepository
{
    public interface ITheaterRepository:IGenericRepository<Theater>
    {
        Task<Theater?> GetTheaterByMovieId(int movieId, bool includeRelated = true);
        Task<Theater?> GetByIdAsync(int theaterId);

    }
}
