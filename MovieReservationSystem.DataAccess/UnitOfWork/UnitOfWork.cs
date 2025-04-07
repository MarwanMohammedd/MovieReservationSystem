using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext applicationDBContext;
        public IMovieRepository Movie { get; }

        public IMovieSchedleRepository MovieSchedle { get; }
        public IReviewRepository Review { get; }

        public UnitOfWork(ApplicationDBContext applicationDBContext, IMovieRepository movieRepository, IMovieSchedleRepository movieSchedleRepository, IReviewRepository review)
        {
            this.applicationDBContext = applicationDBContext;
            Movie = movieRepository;
            MovieSchedle = movieSchedleRepository;
            Review = review;
        }


        public async void Dispose()
        {
            await applicationDBContext.DisposeAsync();
        }
        public async Task Save()
        {
            await applicationDBContext.SaveChangesAsync();
        }

      

    }
}