using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Model.Models{
    public class Ticket : BaseEnitiy
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        [DataType("money")]
        public double Price { get; set; }
        public User? User { get; set; }
        public Movie? Movie { get; set; }
        public Seat Seat { get; set; } = null!;
    }
}