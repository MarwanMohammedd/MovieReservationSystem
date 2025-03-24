using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.Model.Models{
    public class User : BaseEnitiy
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [DataType(dataType: DataType.PhoneNumber)]
        public int PhoneNumber { get; set; } 
        [DataType(dataType: DataType.EmailAddress)]
        public string? EmailAddress { get; set; } 

        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
    }
}