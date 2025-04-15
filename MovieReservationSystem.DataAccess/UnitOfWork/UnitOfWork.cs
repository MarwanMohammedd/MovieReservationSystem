using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DataAccess.Repository.TheaterRepository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext applicationDBContext;
        public IMovieRepository Movie { get; }

        public IMovieSchedleRepository MovieSchedle { get; }
        public IReviewRepository Review { get; }

        public TheaterRepository Theater { get; }

        public UnitOfWork(ApplicationDBContext applicationDBContext, IMovieRepository movieRepository, IMovieSchedleRepository movieSchedleRepository, IReviewRepository review, TheaterRepository theaterRepository)
        {
            this.applicationDBContext = applicationDBContext;
            Movie = movieRepository;
            Theater = theaterRepository;
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