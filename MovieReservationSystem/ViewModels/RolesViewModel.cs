using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MovieReservationSystem.ViewModels
{
    public class RolesViewModel
    {   
        [Required]
        [Display(Name ="Role Name")]
        
        public string RoleName { get; set; } = null!;

        public IEnumerable<IdentityRole<int>>? Roles { get; set; }
    }
}