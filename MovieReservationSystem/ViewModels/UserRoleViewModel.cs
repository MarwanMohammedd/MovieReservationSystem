using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;

namespace MovieReservationSystem.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserEmail { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public int UserId { get; set; } = 0;
        public List<string> AvailableRoles { get; set; } = null!;
    }
}