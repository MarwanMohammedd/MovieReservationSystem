using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservationSystem.Model.Models{
    public class TheatersSchedule
    {
        public int TheaterId { get; set; }
        public int MovieId { get; set; }
        public Theater? Theater { get; set; }
        public Movie? Movie { get; set; }
        
    }
}