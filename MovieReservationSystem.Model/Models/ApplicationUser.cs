using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace MovieReservationSystem.Model.Models{
    public class ApplicationUser : IdentityUser<int>
    {
        public String FullName { get; set; } = null!;
        public IEnumerable<Review>? Reviews { get; set; }
        public IEnumerable<Ticket>? Tickets { get; set; }
    }
}