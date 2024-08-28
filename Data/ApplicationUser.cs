using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(60, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string? first_Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(60, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string? last_Name { get; set; }

        [StringLength(255)]
        public string? parent_id { get; set; }

        public DateTime DOB { get; set; }

        public int TwoFactorEmail { get; set; }

        public int UserId { get; set; }

        public int Approved { get; set; }

        public string? socialSecurityNumber { get; set; }

        public string? middleName { get; set; }

        public string? Avatar { get; set; }

        public int has2fa { get; internal set; }
    }
}
