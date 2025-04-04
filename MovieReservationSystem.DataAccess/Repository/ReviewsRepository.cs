using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository
{
    public class ReviewsRepository : GenericRepository<Review>
    {
        ApplicationDBContext _applicationDBContext;
        public ReviewsRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

    }
}
