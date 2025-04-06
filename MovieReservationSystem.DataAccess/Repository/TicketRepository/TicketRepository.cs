using Microsoft.EntityFrameworkCore;
using MovieReservationSystem.DataAccess.Data;
using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.DataAccess.Repository.TicketRepository
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;

        public TicketRepository(ApplicationDBContext applicationDBContext) : base(applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<Ticket?> GetBySeatAsync(int movieId, string seatNumber)
        {
            return await _applicationDBContext.Tickets
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(t => t.MovieId == movieId && t.Seat.SeatNumber == seatNumber);
        }

        public async Task<IEnumerable<Ticket>> GetByMovieAsync(int movieId)
        {
            return await _applicationDBContext.Tickets
                .Include(t=>t.Seat)
                .Where(t => t.MovieId == movieId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Ticket>> GetExpiredTicketsAsync()
        {
            DateTime now = DateTime.Now;

            return await _applicationDBContext.Tickets
                .Include(t => t.Seat)
            .Include(t => t.Movie)
                .Where(t => _applicationDBContext.MovieSchedules
                                .Any(ms => ms.MovieId == t.MovieId && ms.Showtime < now))
                .ToListAsync();
        }
        public async Task<int> RemoveExpiredTicketsAsync()
        {
            IEnumerable<Ticket> expiredTickets = await GetExpiredTicketsAsync();

            foreach (Ticket ticket in expiredTickets)
            {

                if (ticket.Seat != null)
                {
                    ticket.Seat.IsReserved = false;
                }

              await  Delete(t =>t.ID ==ticket.ID );
       
            }

            return await _applicationDBContext.SaveChangesAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int ticketId)
        {
           return await _applicationDBContext.Tickets
                .Include(t=>t.Seat)
                .FirstOrDefaultAsync(t => t.ID == ticketId);

        }
    }
}
