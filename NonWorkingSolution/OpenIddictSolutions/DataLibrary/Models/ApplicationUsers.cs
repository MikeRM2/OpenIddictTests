using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataLibrary.Models
{
    public class ApplicationUsers : IdentityUser<Guid>
    {
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Middle Initial")]
        [MaxLength(2)]
        public string MiddleIni { get; set; }

        [Display(Name = "Suffix")]
        [MaxLength(5)]
        public string Suffix { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsActive { get; set; }
        
    }
}