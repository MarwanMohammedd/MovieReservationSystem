using System.ComponentModel;

namespace MovieReservationSystem.Model.Models{
    public class Movie : BaseEnitiy
    {
    public string Title { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateOnly ProductionYear { get; set; }        
    public int Duration { get; set; }
    
        public string ImagePath { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;

        public IEnumerable<MovieSchedule>? MovieSchedule { get; set; } 
    public IEnumerable<Review>? Reviews { get; set; } 
    public IEnumerable<Ticket>? Tickets { get; set; } 
    public IEnumerable<TheatersSchedule>? TheatersSchedules { get; set; }
    }
}