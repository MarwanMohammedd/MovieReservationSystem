using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace MovieReservationSystem.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
        
        public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    }
}
