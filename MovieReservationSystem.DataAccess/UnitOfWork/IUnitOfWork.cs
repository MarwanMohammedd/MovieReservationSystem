using MovieReservationSystem.DataAccess.Repository;
using MovieReservationSystem.DataAccess.Repository.TheaterRepository;
using MovieReservationSystem.DataAccess.Repository.TheatorRepository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movie { get; }
        IMovieSchedleRepository MovieSchedle { get; }
        IReviewRepository Review { get; }
        TheaterRepository Theater { get; }
        Task Save();
    }
}