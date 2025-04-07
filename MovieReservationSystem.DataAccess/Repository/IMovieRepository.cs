using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.DataAccess.Repository
{
    
    public interface IMovieRepository :IGenericRepository<Movie>
    {
        //public Task<IEnumerable<Movie>> MoviePagenation(int page ,int size);
        public Task<IEnumerable<Movie>> MoviePagenationOrderBy(int page, int siz, Expression<Func<Movie,object>> Predicate, Expression<Func<Movie, bool>>? PredicateWhere =default);

    }
}
