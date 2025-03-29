using MovieReservationSystem.DataAccess.Repository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {   
        void Save();
    }
}