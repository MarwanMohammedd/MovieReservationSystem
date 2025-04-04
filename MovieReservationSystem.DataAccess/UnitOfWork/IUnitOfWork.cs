using MovieReservationSystem.DataAccess.Repository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {   
        Task Save();
    }
}