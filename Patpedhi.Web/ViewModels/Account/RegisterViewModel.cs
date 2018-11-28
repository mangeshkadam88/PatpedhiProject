using System.ComponentModel.DataAnnotations;
using System;

namespace Patpedhi.Web.ViewModels.Account
{
    public class RegisterViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public string DateofBirth { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public long AccountNo { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Approved")]
        public bool IsApproved { get; set; }

        public string RoleName { get; set; }

        public string ProfilePhotoURL { get; set; }
        public string SignaturePhotoURL { get; set; }
    }
}
