namespace MovieReservationSystem.Model.Models{
    public class Theater : BaseEnitiy
    {
        public string Name { get; set; } = null!;
        public int SeatsCount { get; set; }
        public IEnumerable<TheatersSchedule>? TheatersSchedules { get; set; }
        public IEnumerable<Seat>? Seats { get; set; }
        
    }
}