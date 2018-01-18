using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace FrontEnd.Areas.Accounts.Models
{
    public class AccountViewModel 
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Status { get; set; }

        public string Designation { get; set; }

        //public IEnumerable<IdentityRole> Id { get; set; }


    }

    public class AccountRegistrationModel {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "User Name")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Status { get; set; }

        public string Designation { get; set; }

        public string Id { get; set; }
    }

    
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string LoginDate { get; set; }
    }

    public class UserCredentialsModel
    {
        public string Id { get; set; }

        [Display(Name = "new Password")]
        public string Password { get; set; }

        [Display(Name ="Old Password")]
        public string OldPassword { get; set; }
    }

    public class SystemStatus
    {
        public int SystemId { get; set; }

        public bool Status { get; set; }
    }
    
}
