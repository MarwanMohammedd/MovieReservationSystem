namespace MovieReservationSystem.Model.Models{
   
    public class MovieSchedule
    {
        public int MovieId { get; set; }
        public int StartTime { get; set; }
        public Movie? Movie { get; set; }
    }
}