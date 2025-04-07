using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservationSystem.Model.Models{
    public class Review
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string? Commnet { get; set; }
        public int Rate { get; set; } = 0;
        

        public Movie? Movie { get; set; }
        public ApplicationUser? User { get; set; }
    }
}