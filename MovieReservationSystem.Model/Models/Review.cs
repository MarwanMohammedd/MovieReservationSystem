using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Model.Models{
    public class Review
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string? Commnet { get; set; }
        public int Rate { get; set; } = 0;

        public Movie? Movie { get; set; }
        public User? User { get; set; }
    }
}