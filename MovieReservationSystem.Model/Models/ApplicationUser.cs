using Microsoft.AspNetCore.Identity;

namespace MovieReservationSystem.Model.Models{
    public class ApplicationUser : IdentityUser<int>
    {
        public String FirstName { get; set; } = null!;
        public String LastName { get; set; } = null!;
        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
    }
}