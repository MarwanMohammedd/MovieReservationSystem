using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.ViewModels
{
    public class MovieViewModel
    {
        public int ID { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Director { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public DateOnly ProductionYear { get; set; }
        public int Duration { get; set; } = 0;
        public IEnumerable<Movie>? Movies { get; set; }
    }
}
