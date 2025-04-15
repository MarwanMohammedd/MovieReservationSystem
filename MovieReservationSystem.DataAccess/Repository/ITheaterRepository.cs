using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.TheatorRepository
{
    interface ITheaterRepository:IGenericRepository<Theater>
    {
        Task<Theater?> GetTheaterByMovieId(int movieId, bool includeRelated = true);

    }
}
