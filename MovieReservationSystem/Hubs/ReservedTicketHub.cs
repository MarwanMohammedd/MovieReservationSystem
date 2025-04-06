using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MovieReservationSystem.Hubs
{
    public class ReservedTicketHub : Hub
    {
        public async Task JoinMovieGroup(int movieId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"movie-{movieId}");
        }
    }
}
