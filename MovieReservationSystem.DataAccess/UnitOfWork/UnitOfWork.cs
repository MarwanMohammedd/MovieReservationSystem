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

        public async void Dispose()
        {
         //   await applicationDBContext.DisposeAsync();
        }   
        public async Task Save()
        {
            await applicationDBContext.SaveChangesAsync();
        }
    }
}