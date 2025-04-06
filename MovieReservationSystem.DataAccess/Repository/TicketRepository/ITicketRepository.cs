using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.TicketRepository
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        public Task<Ticket?> GetByIdAsync(int ticketId);
        public  Task<Ticket?> GetBySeatAsync(int movieId, string seatNumber);
        public Task<IEnumerable<Ticket>> GetByMovieAsync(int movieId);
        public Task<IEnumerable<Ticket>> GetExpiredTicketsAsync();
        public  Task<int> RemoveExpiredTicketsAsync();

    }
}
