using MovieReservationSystem.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieReservationSystem.Model.ViewModels
{
    public class MovieTheaterViewModel
    {
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = null!;
        public string MovieGenre { get; set; } = null!;
        public string MovieDescription { get; set; } = null!;
        public DateOnly MovieProductionYear { get; set; }
        public int MovieDuration { get; set; }


        public int TheaterId { get; set; }
        public string TheaterName { get; set; } = null!;
        public int TheaterSeatsCount { get; set; }
        public IEnumerable<Seat> TheaterSeats { get; set; } = null!;
    }
}
