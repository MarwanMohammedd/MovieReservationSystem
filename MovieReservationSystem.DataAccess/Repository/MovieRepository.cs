using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.DataAccess.Repository
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        
        private readonly ApplicationDBContext applicationDBContext;

        public MovieRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
            this.applicationDBContext = applicationDBContext;
        }

        public async Task<IEnumerable<Movie>> MoviePagenationOrderBy(int page, int size,
            Expression<Func<Movie, object>> Predicate, Expression<Func<Movie, bool>> PredicateWhere=null )
        {

            IQueryable<Movie> query = applicationDBContext.Movies;

            if (PredicateWhere != null)
            {
                query = query.Where(PredicateWhere);
            }

            var movies = await query
                .OrderByDescending(Predicate)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();



            return  movies;
        }
    }
}
