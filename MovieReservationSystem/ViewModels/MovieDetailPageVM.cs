using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.ViewModels
{
    public class MovieDetailPageVM
    {
        public int ID { get; set; }
        public string Title { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly ProductionYear { get; set; }
        public int Duration { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public IEnumerable<DateTime>? MovieScheduleList { get; set; }

        public IEnumerable<Review>? reviews { get; set; }
    }
}
