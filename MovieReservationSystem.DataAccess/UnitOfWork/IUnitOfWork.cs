using MovieReservationSystem.DataAccess.Repository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IMovieRepository Movie { get; }
        IMovieSchedleRepository MovieSchedle { get; }
        IReviewRepository Review { get; }
        Task Save();
    }
}