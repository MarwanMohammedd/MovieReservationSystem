namespace MovieReservationSystem.Model.Models{
   
    public class MovieSchedule
    {
        public int MovieId { get; set; }
        public DateTime Showtime { get; set; }
        public Movie? Movie { get; set; }
    }
}