using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieReservationSystem.Model.Models{
    public class Ticket : BaseEnitiy
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int MovieId { get; set; }
        [DataType("money")]
        public double Price { get; set; }
        public ApplicationUser? User { get; set; }
        public Movie? Movie { get; set; }
        public Seat Seat { get; set; } = null!;
        
    }
}