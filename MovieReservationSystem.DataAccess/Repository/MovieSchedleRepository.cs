using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository
{
    public class MovieSchedleRepository : GenericRepository<MovieSchedule>, IMovieSchedleRepository
    {
        
        private readonly ApplicationDBContext applicationDBContext;

        public MovieSchedleRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }
    }
}
