using MovieReservationSystem.Model.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.ViewModels
{
    public class MovieScheduleVM
    {
      public int ID { get; set; }

        public DateTime StartTime { get; set; }
        
        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public IEnumerable<Movie>? MovieList { get; set; }
    }
}
