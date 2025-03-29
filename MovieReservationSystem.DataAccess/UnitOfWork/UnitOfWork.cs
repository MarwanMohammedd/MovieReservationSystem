using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.DataAccess.Repository;

namespace MovieReservationSystem.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext applicationDBContext;
        public UnitOfWork(ApplicationDBContext applicationDBContext){
            this.applicationDBContext=applicationDBContext;
        }

        public void Dispose()
        {
            applicationDBContext.Dispose();
        }
        public void Save()
        {
            applicationDBContext.SaveChanges();
        }
    }
}