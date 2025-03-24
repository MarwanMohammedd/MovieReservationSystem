using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservationSystem.Model.Models{
   
    public class MovieSchedule
    {
        public int MovieId { get; set; }
        public int StartTime { get; set; }
        public Movie? Movie { get; set; }
    }
}