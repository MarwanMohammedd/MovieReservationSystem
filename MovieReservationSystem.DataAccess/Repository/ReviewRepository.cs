using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository
{
    public class ReviewRepository:GenericRepository<Review>, IReviewRepository
    {
        
        private readonly ApplicationDBContext applicationDBContext;

        public ReviewRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }
    }
   
}
