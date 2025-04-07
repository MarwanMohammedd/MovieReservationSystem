using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Model.Models{
   
    public class MovieSchedule :BaseEnitiy
    {
        public int MovieId { get; set; }
        public DateTime StartTime { get; set; }
        
        public Movie? Movie { get; set; }
    }
}