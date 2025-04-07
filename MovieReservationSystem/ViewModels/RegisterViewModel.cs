using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MovieReservationSystem.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First Name Is Required")]
        //[MinLength(6, ErrorMessage = "Must More Than 6 Charachters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name Is Required")]
       // [MinLength(6, ErrorMessage = "Must More Than 6 Charachters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Remote(action: "IsUserNameAvailable", controller: "Account")]
        [Required(ErrorMessage = "User Name Is Required")]
        [MinLength(6, ErrorMessage = "Must More Than 6 Charachters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        [Remote(action: "IsEmailAvailable", controller: "Account")]
        [Required(ErrorMessage = "Email Address Is Required")]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; } = null!;

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirmation Password Is Required")]
        [Compare("Password", ErrorMessage = "Confirmation Password Not Match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmation Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
