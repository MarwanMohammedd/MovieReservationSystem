namespace MovieReservationSystem.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserEmail { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        
        public List<string> AvailableRoles { get; set; } = null!;
    }
}