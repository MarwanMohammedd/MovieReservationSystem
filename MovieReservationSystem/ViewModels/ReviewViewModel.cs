using MovieReservationSystem.Model.Models;

namespace MovieReservationSystem.ViewModels
{
    public class ReviewViewModel
    {
        public int? UserId { get; set; }
        public int MovieId { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public int Rate { get; set; } = 0;

        public IEnumerable<Review>? MovieReviews { get; set; }


    }
}
